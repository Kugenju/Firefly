# Firefly Mod 迭代路线图

> 基于设计文档 v1.0 与当前实现差距分析
> 分析时间: 2026年4月9日

---

## 一、当前实现状态

### ✅ 已完成

| 类别 | 内容 | 状态 |
|------|------|------|
| **角色框架** | Firefly 类继承 PlaceholderCharacterModel | ✅ 完整 |
| **基础卡牌** | 13张卡牌基础框架 | ✅ 有框架，缺机制 |
| **遗物框架** | 6个遗物基础定义 | ✅ 有定义，缺效果 |
| **本地化** | 中文本地化文件 | ✅ 完整 |
| **卡池配置** | 卡牌池、遗物池、药水池 | ✅ 完整 |

### ❌ 缺失的核心机制

| 机制 | 设计文档描述 | 当前状态 | 影响度 |
|------|-------------|---------|--------|
| **灼热 (Scorch)** | 敌人攻击意图时受到伤害，层数-1 | ❌ 未实现 | 🔴 核心机制 |
| **完全燃烧** | 3回合强化状态，攻击力+3，多抽牌 | ❌ 未实现 | 🔴 核心机制 |
| **生命消耗** | 部分卡牌消耗生命值 | ❌ 未实现 | 🟡 重要 |
| **遗物效果** | 萨姆装甲的萤火护盾和荆棘 | ❌ 未实现 | 🟡 重要 |

---

## 二、迭代方向分析

### 方向1: 实现灼热机制 (Scorch) 🔥 **推荐首选**

**实现内容:**
```csharp
// 新增 ScorchPower 能力
public class ScorchPower : PowerModel
{
    // 敌人显示攻击意图时触发
    public override void OnEnemyIntentShown(CombatContext ctx)
    {
        // 造成等于层数的伤害
        // 层数-1
    }
}
```

**实现难度:** ⭐⭐ (中等)
- 需要创建新的 Power 类
- 需要 Hook 敌人意图显示时机
- 需要添加伤害计算

**影响度:** 🔴🔴🔴 (极高)
- 这是角色的核心战斗机制
- 13张卡牌中有8张涉及灼热
- 决定角色的独特玩法

**所需技能:**
- 查找游戏 Hook 点
- 创建 Power 能力系统
- 伤害计算和状态管理

---

### 方向2: 实现完全燃烧机制 💥 **推荐第二**

**实现内容:**
```csharp
// 新增 CompleteCombustionPower 能力
public class CompleteCombustionPower : PowerModel
{
    // 持续3回合
    // 每回合开始时：获得3点临时力量，抽1张额外牌
    // 追踪剩余回合数
}

// 修改基础卡牌在完全燃烧状态下的效果
public class FireflyStrike : CardModel
{
    // 检测是否有 CompleteCombustionPower
    // 如果有：伤害+4，施加灼热
}
```

**实现难度:** ⭐⭐⭐ (较难)
- 需要回合追踪机制
- 需要临时力量系统
- 需要卡牌效果动态变化
- 需要形态转换UI提示

**影响度:** 🔴🔴🔴 (极高)
- 这是角色的爆发机制
- 决定战斗节奏（3回合爆发窗口）
- 与灼热机制形成Combo

---

### 方向3: 实现生命消耗机制 💔 **推荐第三**

**实现内容:**
```csharp
// 修改卡牌，添加生命消耗
public class EmberBlade : CardModel
{
    protected override async Task OnPlay(...)
    {
        // 先失去生命值
        await PlayerTakeDamage(3);
        // 再造成伤害
        await DealDamage(...);
    }
}
```

**实现难度:** ⭐⭐ (中等)
- 相对简单，主要是伤害自己
- 需要注意生命值不足时的处理

**影响度:** 🟡🟡 (中等)
- 增加风险/收益博弈
- 强化角色特色
- 与完全燃烧形成策略组合

---

### 方向4: 完善遗物效果 🛡️ **推荐第四**

**实现内容:**
```csharp
// 萨姆装甲效果
public class SamArmor : RelicModel
{
    // 每打出3张牌，获得萤火护盾
    // 生命值低于20点时，获得荆棘
}
```

**实现难度:** ⭐⭐ (中等)
- 计数器追踪
- 条件触发逻辑
- 护盾系统

**影响度:** 🟡🟡 (中等)
- 增强角色生存能力
- 但初始遗物不是核心玩法

---

### 方向5: 添加缺失的卡牌 🔧 **推荐第五**

根据设计文档，缺失的卡牌：

| 卡牌名 | 类型 | 费用 | 核心效果 |
|--------|------|------|---------|
| 能量超载 | 技能 | 0 | 获得能量，下回合失去 |
| 偏时迸发 | 技能 | 1 | 下一张攻击无视格挡 |
| 自限装甲 | 能力 | 2 | 低血量时获得荆棘 |
| 过载核心 | 能力 | 2 | 失去生命时获得力量 |
| 萤火之舞 | 技能 | 1 | 抽牌，下一张攻击0费 |
| 永燃之心 | 能力 | 3 | 每回合施加灼热 |

**实现难度:** ⭐⭐ (中等，每张)
**影响度:** 🟢 (低-中等)
- 丰富卡牌池
- 但不是核心机制

---

### 方向6: 美术资源 🎨 **推荐最后**

- 角色立绘
- 卡牌插图
- 动画效果

**实现难度:** ⭐⭐⭐⭐ (很高)
- 需要美术技能
- 需要Spine动画

**影响度:** 🟢 (体验提升)
- 提升视觉体验
- 但不影响游戏性

---

## 三、推荐迭代顺序

### Phase 1: 核心机制 (第1-2周)

**目标:** 让角色有可玩的独特机制

1. **实现灼热 (Scorch) 机制**
   - 创建 ScorchPower 类
   - Hook 敌人意图显示
   - 修改相关卡牌施加灼热

2. **实现完全燃烧机制**
   - 创建 CompleteCombustionPower 类
   - 实现回合追踪
   - 修改基础卡牌在完全燃烧下的效果

**预期成果:**
- 角色有独特的战斗循环
- 可以体验到 "叠加灼热 → 触发完全燃烧 → 爆发输出" 的玩法

---

### Phase 2: 机制完善 (第3-4周)

**目标:** 丰富玩法，增加策略深度

3. **实现生命消耗机制**
   - 修改燃尽之刃、天火轰击等卡牌
   - 添加生命值检查

4. **实现萨姆装甲效果**
   - 萤火护盾计数器
   - 低血量荆棘效果

**预期成果:**
- 风险/收益决策
- 生存能力提升
- 战斗更有层次感

---

### Phase 3: 内容扩展 (第5-6周)

**目标:** 增加卡牌多样性

5. **添加缺失的6张卡牌**
   - 能量超载
   - 偏时迸发
   - 自限装甲
   - 过载核心
   - 萤火之舞
   - 永燃之心

**预期成果:**
- 更多构筑可能
- 丰富策略选择

---

### Phase 4: 体验优化 (第7-8周)

**目标:** 提升整体体验

6. **Bug修复和平衡调整**
   - 数值平衡
   - Edge case处理

7. **美术资源** (可选)
   - 卡牌插图
   - 角色动画

---

## 四、技术实现建议

### 灼热机制实现路径

```csharp
// 1. 创建 ScorchPower.cs
public class ScorchPower : PowerModel
{
    public ScorchPower()
    {
        Name = "灼热";
        Type = PowerType.Debuff;
    }
    
    // 在敌人显示攻击意图时触发
    public override void OnIntentShown(CombatContext ctx)
    {
        // 造成等于层数的伤害
        Owner.TakeDamage(Amount);
        // 层数-1
        ReducePower(1);
    }
}

// 2. 修改卡牌施加灼热
public class FlameLash : CardModel
{
    protected override async Task OnPlay(...)
    {
        // 造成伤害
        await DealDamage(...);
        // 施加灼热
        await ApplyPower<ScorchPower>(2);
    }
}
```

### 完全燃烧机制实现路径

```csharp
// 1. 创建 CompleteCombustionPower.cs
public class CompleteCombustionPower : PowerModel
{
    public override void OnTurnStart(CombatContext ctx)
    {
        // 获得临时力量
        Owner.AddTemporaryPower<StrengthPower>(3);
        // 额外抽牌
        Owner.DrawCards(1);
        // 减少剩余回合
        // 如果回合结束，移除能力
    }
}

// 2. 修改基础卡牌检测状态
public class FireflyStrike : CardModel
{
    protected override async Task OnPlay(...)
    {
        var damage = DynamicVars.Damage.BaseValue;
        
        // 检测是否有完全燃烧
        if (Owner.HasPower<CompleteCombustionPower>())
        {
            damage += 4; // 额外伤害
            // 施加灼热
            await ApplyPower<ScorchPower>(2);
        }
        
        await DealDamage(damage);
    }
}
```

---

## 五、快速启动建议

如果你希望**快速看到效果**，建议：

### 最小可行产品 (MVP)

只实现 **灼热机制的基础版本**：

1. 创建 `ScorchPower` - 简单的伤害触发
2. 修改 3 张卡牌施加灼热
3. 测试战斗循环

**预计工作量:** 2-3小时
**效果:** 立即看到角色的核心玩法

### 下一个步骤

1. 确定要优先实现哪个机制
2. 我可以帮你编写具体的实现代码
3. 测试并调整数值平衡

---

## 六、决策建议

| 你的情况 | 建议方向 | 理由 |
|---------|---------|------|
| 想快速看到成果 | 灼热机制 MVP | 2-3小时，核心玩法 |
| 想完整体验角色 | Phase 1 全部 | 核心机制完整 |
| 时间有限 | 灼热 + 完全燃烧 | 最重要的两个机制 |
| 想丰富内容 | Phase 3 卡牌 | 增加构筑深度 |

**我的推荐:** 先做 Phase 1（灼热 + 完全燃烧），这两个机制决定了角色是否好玩。

你想从哪个方向开始？我可以帮你实现具体的代码。
