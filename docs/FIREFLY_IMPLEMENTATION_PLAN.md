# 萤火(Firefly)机制实现方案

## 机制概述

萤火是流萤卡组的核心卡牌属性机制，类似猎手的「奇巧」牌。

### 核心机制
- **萤火关键词**: 卡牌固有属性，可在卡牌上显示
- **两种状态**:
  - 未激发: 普通状态，正常效果，正常耗能
  - 激发: 效果翻倍，耗能-1（至少1点）
- **触发条件**: 打出「完全燃烧」会激发所有手牌中的萤火牌

### 视觉表现
- 未激发: 卡牌边框呈淡蓝色微光
- 激发: 卡牌边框燃烧绿色火焰，并有萤火虫粒子特效

---

## 实现状态

### ✅ Phase 1: 关键词定义 - 已完成
- **文件**: `Scripts/Keywords/FireflyKeywords.cs`
- **本地化**: `Firefly/localization/zhs/card_keywords.json`

### ✅ Phase 2: 萤火状态管理 - 已完成
**实现方案**: 静态状态管理器（Power + FireflyIgnitionManager）

由于BaseLib未提供CustomEnchantmentModel，采用简化方案：
- `CompleteCombustionPower` - 临时Power（3回合）
- `FireflyIgnitionManager` - 静态类跟踪激发状态

**文件**: `Scripts/Powers/CompleteCombustionPower.cs`

```csharp
public static class FireflyIgnitionManager
{
    public static void IgniteCard(CardModel card);
    public static bool IsIgnited(CardModel card);
    public static int GetEffectMultiplier(CardModel card);
    public static void ClearAllIgnitions();
}
```

### ✅ Phase 3: 完全燃烧Power重做 - 已完成
**文件**: `Scripts/Powers/CompleteCombustionPower.cs`

效果：
- 激发手中所有萤火牌
- 每回合多抽1张牌
- 受到的伤害减少5点
- 持续3回合

### ✅ Phase 4: 萤火专属卡牌 - 已完成

| 卡牌 | 类型 | 稀有度 | 效果 | 状态 |
|------|------|--------|------|------|
| `FlashIgniteStrike` | 攻击 | Common | 造成8/12伤害 | ✅ |
| `ArmoredDefense` | 技能 | Common | 获得8/12格挡 | ✅ |
| `FireflyWildfire` | 技能 | Uncommon | 抽2/3张，每萤火牌额外抽1 | ✅ |
| `IgniteTheSea` | 技能 | Rare | 激发手牌中所有萤火牌 | ✅（重做） |
| `FlamesSpread` | 技能 | Rare | 打出手牌中所有萤火牌 | ✅（新增） |

**文件**: 
- `Scripts/Cards/FlashIgniteStrike.cs`
- `Scripts/Cards/ArmoredDefense.cs`
- `Scripts/Cards/FireflyWildfire.cs`
- `Scripts/Cards/IgniteTheSea.cs`（重做）
- `Scripts/Cards/FlamesSpread.cs`（新增）
- `Scripts/Cards/CompleteCombustionCard.cs`（更新）

### ⏳ Phase 5: 视觉特效 - 待实现
等待美术资源或Shader实现。

---

## 技术实现细节

### FireflyIgnitionManager工作原理

```csharp
// 激发一张卡片
public static void IgniteCard(CardModel card)
{
    var instanceId = GetCardInstanceId(card);
    
    if (_ignitedCardIds.Add(instanceId))
    {
        // 记录原始能耗
        _originalEnergyCosts[instanceId] = card.EnergyCost.BaseCost;
        
        // 减少能耗（最小为1）
        int newCost = Math.Max(1, card.EnergyCost.BaseCost - 1);
        card.EnergyCost.SetTemporaryCost(newCost);
        
        // 通知能耗变化
        card.InvokeEnergyCostChanged();
    }
}
```

### 卡牌使用激发效果

```csharp
protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
{
    // 检查是否被激发
    int multiplier = FireflyIgnitionManager.GetEffectMultiplier(this);
    
    // 获取基础伤害
    int baseDamage = DynamicVars.GetVar<int>("Damage");
    int damage = baseDamage * multiplier;

    // 造成伤害
    await DamageCmd.Give(choiceContext, damage, cardPlay.Target, this);
}
```

---

## 文件清单

| 文件 | 说明 | 状态 |
|------|------|------|
| `Scripts/Keywords/FireflyKeywords.cs` | 萤火关键词定义 | ✅ |
| `Scripts/Powers/CompleteCombustionPower.cs` | 完全燃烧Power | ✅ |
| `Scripts/Hooks/FireflyCombatEndHook.cs` | 战斗结束清理钩子 | ✅ |
| `Scripts/Cards/CompleteCombustionCard.cs` | 完全燃烧卡牌 | ✅ |
| `Scripts/Cards/FlashIgniteStrike.cs` | 闪燃打击 | ✅ |
| `Scripts/Cards/ArmoredDefense.cs` | 装甲防御 | ✅ |
| `Scripts/Cards/FireflyWildfire.cs` | 萤火燎原 | ✅ |
| `Scripts/Cards/IgniteTheSea.cs` | 点燃大海（重做） | ✅ |
| `Scripts/Cards/FlamesSpread.cs` | 海一直燃（新增） | ✅ |
| `Firefly/localization/zhs/card_keywords.json` | 关键词本地化 | ✅ |
| `Firefly/localization/zhs/cards.json` | 卡牌本地化 | ✅ |
| `Firefly/localization/zhs/powers.json` | Power本地化 | ✅ |

---

## 后续优化项

1. **视觉特效**: 激发状态的卡牌边框发光效果
2. **清理逻辑**: 确定激发状态是否在战斗结束后清除
3. **测试验证**: 在实际游戏中测试所有卡牌效果
