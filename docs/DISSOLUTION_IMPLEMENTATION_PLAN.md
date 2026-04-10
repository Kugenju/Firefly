# 裂解 (Dissolution) 机制实现方案

## 核心机制回顾

- **触发条件**: 敌人的格挡从有到无被击破时（从>0变为=0）
- **伤害计算**: 受到等同于**本回合该敌人获得的所有格挡总数**的伤害
- **视觉表现**: 格挡破碎时产生绿色火焰/粒子特效

## 实现方案

### 1. 核心Power - DissolutionPower (裂解标记)

这是一个Debuff类型的Power，标记敌人已触发裂解状态，用于后续卡牌效果判断。

```csharp
public class DissolutionPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Single;
    
    // 标记敌人已经被裂解（用于裂解打击等卡牌判断）
}
```

### 2. 核心Power - DissolutionSourcePower (裂解源)

这个Power负责：
1. 追踪敌人本回合获得的格挡总数
2. 监听格挡获得事件
3. 在格挡被打破时触发裂解伤害
4. 回合结束时重置计数

```csharp
public class DissolutionSourcePower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff; // 内部使用，对玩家不可见
    public override PowerStackType StackType => PowerStackType.Single;
    
    // 本回合获得的格挡总数
    private int _blockGainedThisTurn = 0;
    
    // 回合开始时重置计数
    public override Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side == Owner.Side)
        {
            _blockGainedThisTurn = 0;
        }
        return Task.CompletedTask;
    }
    
    // 记录获得的格挡
    public override Task AfterBlockGained(Creature creature, decimal amount, ValueProp props, CardModel cardSource)
    {
        if (creature == Owner && amount > 0)
        {
            _blockGainedThisTurn += (int)amount;
        }
        return Task.CompletedTask;
    }
    
    // 格挡被打破时触发裂解伤害
    public override async Task AfterBlockBroken(Creature creature)
    {
        if (creature == Owner && _blockGainedThisTurn > 0)
        {
            // 触发裂解伤害
            await TriggerDissolutionDamage();
        }
    }
}
```

### 3. 关键Hook方法

根据源码分析，使用以下Hook：

| Hook方法 | 用途 |
|----------|------|
| `AfterBlockGained` | 追踪本回合获得的格挡数 |
| `AfterBlockBroken` | 格挡被打破时触发裂解伤害 |
| `AfterSideTurnStart` | 回合开始时重置计数 |

### 4. 卡牌实现要点

#### 熵增转移 (EntropyTransfer)
```csharp
// 1. 获得自身格挡
// 2. 将自身格挡转移给敌人（使用CreatureCmd.GiveBlock的反向逻辑或Harmony补丁）
// 3. 给敌人应用DissolutionSourcePower（如果不存在）
// 4. 可选：直接赋予DissolutionPower标记
```

#### 结构崩解 (StructuralCollapse)
```csharp
// 1. 造成伤害
// 2. 如果敌人有格挡，修改伤害计算使其对格挡造成额外伤害
// 使用DamageCmd.Attack配合适当的ValueProp
```

### 5. 技术难点

#### 难点1: 转移格挡给敌人
游戏原生API没有直接支持将玩家格挡转移给敌人。可能的解决方案：
- **方案A**: Harmony补丁修改CreatureCmd.GainBlock行为
- **方案B**: 使用Power系统，创建一个临时Power来模拟敌人获得格挡
- **方案C**: 简化设计，改为"使敌人获得X点格挡"而不是转移

**推荐方案B**: 创建一个`TransferredBlockPower`，在敌人身上模拟格挡效果。

#### 难点2: 对格挡造成多倍伤害
需要修改伤害计算逻辑，让伤害优先攻击格挡且造成多倍伤害。

可能的方案：
- 使用Harmony补丁修改伤害计算
- 或者分两次造成伤害：一次专门破盾，一次造成实际伤害

#### 难点3: 追踪本回合格挡数
需要在Power中维护状态（`_blockGainedThisTurn`），注意：
- Power是会被序列化的，但临时数据不需要持久化
- 使用`[NonSerialized]`标记临时字段

### 6. 实现步骤

1. **Phase 1**: 实现DissolutionPower和DissolutionSourcePower基础框架
2. **Phase 2**: 实现裂解伤害触发逻辑（AfterBlockBroken）
3. **Phase 3**: 实现熵增转移卡牌（基础版本）
4. **Phase 4**: 实现其他裂解相关卡牌

### 7. 代码结构

```
Scripts/
├── Powers/
│   ├── ScorchPower.cs
│   ├── DissolutionPower.cs        # 裂解标记
│   └── DissolutionSourcePower.cs  # 裂解源（追踪格挡）
├── Cards/
│   └── Dissolution/
│       ├── EntropyTransfer.cs     # 熵增转移
│       ├── EntropyArmor.cs        # 熵减装甲
│       ├── StructuralCollapse.cs  # 结构崩解
│       ├── DissolutionStrike.cs   # 裂解打击
│       └── EntropyReversal.cs     # 熵流逆转
└── Patches/
    └── BlockTransferPatch.cs      # 格挡转移Harmony补丁（如需要）
```

## 参考代码

### 游戏原生的相关Hook
```csharp
// AbstractModel.cs
public virtual Task AfterBlockGained(Creature creature, decimal amount, ValueProp props, CardModel cardSource);
public virtual Task AfterBlockBroken(Creature creature);
```

### 游戏原生的伤害命令
```csharp
// CreatureCmd.Damage
public static async Task Damage(
    PlayerChoiceContext choiceContext,
    Creature target,
    decimal amount,
    ValueProp props,
    Creature dealer,
    CardModel cardSource
)
```

### BurrowedPower参考
```csharp
public override async Task AfterBlockBroken(Creature creature)
{
    if (creature == base.Owner)
    {
        await CreatureCmd.TriggerAnim(base.Owner, "UnburrowAttack", 0.25f);
        await CreatureCmd.Stun(base.Owner, "BITE_MOVE");
        await PowerCmd.Remove<BurrowedPower>(base.Owner);
    }
}
```
