# 卡牌开发规范指南

> 基于 BaseLib 框架的卡牌开发规范

## 目录

1. [添加新卡牌关键词](#1-添加新卡牌关键词)
2. [添加新动态变量](#2-添加新动态变量)
3. [添加卡牌提示文本](#3-添加卡牌提示文本)
4. [添加卡牌Tag](#4-添加卡牌tag)

---

## 1. 添加新卡牌关键词

卡牌关键词指的是类似「消耗」「虚无」「萤火」一类的卡牌属性。塔2不需要在卡牌描述里直接写这些，只需在 `CanonicalKeywords` 中添加即可。

### 步骤

#### 1.1 创建关键词定义类

使用 `CustomEnum` 为枚举添加新的值：

```csharp
public class FireflyKeywords
{
    // 自定义枚举的名字。最终会变成 {前缀}-{枚举值大写} 的形式，例如 FIREFLY-FIREFLY
    [CustomEnum("FIREFLY")]
    // 放在原版卡牌描述的位置，这里是卡牌描述的前面
    [KeywordProperties(AutoKeywordPosition.Before)]
    public static CardKeyword Firefly;
}
```

**参数说明**：
- `CustomEnum("PREFIX")`: 定义前缀，最终键名为 `PREFIX-ENUMNAME`
- `AutoKeywordPosition.Before`: 关键词显示在卡牌描述前面
- `AutoKeywordPosition.After`: 关键词显示在卡牌描述后面

#### 1.2 添加本地化文件

创建 `{modId}/localization/{Language}/card_keywords.json`：

```json
{
    "FIREFLY-FIREFLY.description": "可被「完全燃烧」激发。激发后效果翻倍，耗能-1。",
    "FIREFLY-FIREFLY.title": "萤火"
}
```

#### 1.3 在卡牌类中应用

```csharp
[Pool(typeof(FireflyCardPool))]
public class FlashBurnStrike : CustomCardModel
{
    // 添加关键词
    public override IEnumerable<CardKeyword> CanonicalKeywords => [FireflyKeywords.Firefly];
    
    // 或者多个关键词
    public override IEnumerable<CardKeyword> CanonicalKeywords => 
        base.CanonicalKeywords.Concat([FireflyKeywords.Firefly]);
}
```

---

## 2. 添加新动态变量

动态变量是指伤害、格挡、抽牌数、获得能量数等动态数值。虽然可以通过 `new DynamicPower("xxx", 1)` 添加，但写一个新类更规范且便于扩展。

### 步骤

#### 2.1 创建动态变量类

通过 BaseLib 的 `WithTooltip` 可以添加 tooltip。

```csharp
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Firefly.Localization.DynamicVars;

public class ScorchVar : DynamicVar
{
    // 在描述中用作占位符的键，推荐添加前缀避免撞车
    public const string Key = "Firefly-Scorch";
    // 本地化键，这里设置为大写的Key，也就是 "FIREFLY-SCORCH"
    public static readonly string LocKey = Key.ToUpperInvariant();

    public ScorchVar(decimal baseValue) : base(Key, baseValue)
    {
        // 可选：添加tooltip
        this.WithTooltip(LocKey);
    }
}
```

#### 2.2（可选）添加本地化提示

创建 `{modId}/localization/{Language}/static_hover_tips.json`：

```json
{
    "FIREFLY-SCORCH.description": "灼热会在敌人显示攻击意图时造成伤害。",
    "FIREFLY-SCORCH.title": "灼热"
}
```

#### 2.3 在卡牌中使用

```csharp
[Pool(typeof(FireflyCardPool))]
public class FlameLash : CustomCardModel
{
    // 定义动态变量
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(12, ValueProp.Move),
        new ScorchVar(3)  // 使用自定义变量
    ];
    
    // 或者使用简单的动态变量（无tooltip）
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(12, ValueProp.Move),
        new DynamicVar("Firefly-Scorch", 3m)
        // .WithTooltip("FIREFLY-SCORCH") // 如果需要加本地化
    ];
}
```

#### 2.4 在描述中使用

```json
{
    "FIREFLY-FLAME_LASH.title": "火萤斩击",
    "FIREFLY-FLAME_LASH.description": "造成{Damage:diff()}点伤害。\n施加{Firefly-Scorch:diff()}层灼热。"
}
```

**特殊标记说明**：
- `:diff()`: 数值与基础值不同时变红/绿色（升级时显示绿色）
- `:d()`: 强制显示数字格式
- `:p()`: 显示为百分比

---

## 3. 添加卡牌提示文本

提示文本指卡牌旁出现的提示方框，或预览卡牌时的额外信息。通常与关键词染色搭配使用。

### 实现方式

在卡牌类中重载 `ExtraHoverTips`：

```csharp
[Pool(typeof(FireflyCardPool))]
public class CompleteCombustionCard : CustomCardModel
{
    // 通过 HoverTipFactory 添加各种提示文本
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        // 引用其他卡牌作为提示
        HoverTipFactory.FromCard<FlashBurnStrike>(),
        // 引用Power作为提示
        HoverTipFactory.FromPower<ScorchPower>(),
        // 引用关键词作为提示
        HoverTipFactory.FromKeyword(FireflyKeywords.Firefly),
        // 直接创建文本提示
        new HoverTip(this, "额外提示文本", false)
    ];
}
```

---

## 4. 添加卡牌Tag

Tag 是指「打击」「防御」这类标签，用于被其他卡牌或遗物识别（如打击木偶会增伤打击牌）。

### 步骤

#### 4.1 创建Tag定义类

使用 `CustomEnum` 添加新的值：

```csharp
public class FireflyCardTags
{
    [CustomEnum]
    public static CardTag FireflyStrike;  // 萤火打击
    
    [CustomEnum]
    public static CardTag FireflyDefend;  // 萤火防御
}
```

#### 4.2 在卡牌中应用

```csharp
[Pool(typeof(FireflyCardPool))]
public class FlashBurnStrike : CustomCardModel
{
    // 添加tag
    public override IEnumerable<CardTag> Tags => [FireflyCardTags.FireflyStrike];
    
    // 或者继承基础tag
    public override IEnumerable<CardTag> Tags => 
        base.Tags.Concat([FireflyCardTags.FireflyStrike]);
}
```

#### 4.3 判断Tag

```csharp
// CardModel 类型
if (card.Tags.Contains(FireflyCardTags.FireflyStrike))
{
    // 是萤火打击牌
}
```

---

## 快速参考

| 功能 | 类/方法 | 本地化文件 |
|------|---------|-----------|
| 关键词 | `CustomEnum` + `KeywordProperties` | `card_keywords.json` |
| 动态变量 | 继承 `DynamicVar` | `static_hover_tips.json` (可选) |
| 提示文本 | `ExtraHoverTips` + `HoverTipFactory` | - |
| Tag | `CustomEnum` | - |

## 文件命名规范

```
localization/
├── zhs/
│   ├── card_keywords.json      # 卡牌关键词
│   ├── static_hover_tips.json  # 静态提示
│   ├── cards.json              # 卡牌描述
│   └── powers.json             # Power描述
```

## 命名前缀规范

为避免与其他Mod冲突，所有键名应添加Mod前缀：

```
FIREFLY-FLAME_LASH        # 卡牌
FIREFLY-SCORCH            # Power
FIREFLY-FIREFLY           # 关键词
Firefly-Scorch            # 动态变量Key
```
