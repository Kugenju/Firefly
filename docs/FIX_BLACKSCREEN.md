# 修复黑屏卡死问题

## 问题根源

当前 `Firefly` 类继承自 `CustomCharacterModel`，这要求提供**所有**资源路径。缺少任何一个资源都会导致游戏尝试加载默认命名资源而失败。

## 解决方案

根据教程，应该改用 **`PlaceholderCharacterModel`**。

### PlaceholderCharacterModel 的优势

1. **自动使用原版资源** - 你没有的资源直接注释掉即可，会自动使用铁甲战士(Ironclad)的资源
2. **渐进式开发** - 可以逐个添加自定义资源，不需要一次性准备所有资源
3. **避免黑屏** - 不会因为缺少某个资源而卡死

## 需要修改的代码

### 1. 修改 Firefly.cs

```csharp
// 从
public sealed class Firefly : CustomCharacterModel

// 改为
public sealed class Firefly : PlaceholderCharacterModel
```

### 2. 简化资源路径

保留需要的资源路径，其他注释掉：

```csharp
public class Firefly : PlaceholderCharacterModel
{
    // ===== 必须设置的 =====
    public override Color NameColor => new Color("E85D04");
    public override Color EnergyLabelOutlineColor => new Color("D00000");
    public override CharacterGender Gender => CharacterGender.Feminine;
    public override int StartingHp => 70;
    public override int StartingGold => 99;
    
    // 池子
    public override CardPoolModel CardPool => ModelDb.CardPool<FireflyCardPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<FireflyPotionPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<FireflyRelicPool>();
    
    // 初始牌组和遗物
    public override IEnumerable<CardModel> StartingDeck => ...
    public override IReadOnlyList<RelicModel> StartingRelics => ...
    
    // ===== 可选资源（未设置则使用原版）=====
    
    // 角色选择界面
    public override string CustomCharacterSelectIconPath => "res://images/packed/character_select/char_select_ironclad.png";
    public override string CustomCharacterSelectLockedIconPath => "res://images/packed/character_select/char_select_ironclad_locked.png";
    public override string CustomCharacterSelectBg => "res://scenes/screens/char_select/char_select_bg_ironclad.tscn";
    
    // 其他资源暂时注释掉，使用原版
    // public override string CustomVisualPath => ...
    // public override string CustomEnergyCounterPath => ...
    // public override string CustomTrailPath => ...
    // public override string CustomRestSiteAnimPath => ...
    // public override string CustomMerchantAnimPath => ...
    // public override string CustomMapMarkerPath => ...
    
    // 音效 - 使用原版
    public override string CharacterTransitionSfx => "event:/sfx/ui/wipe_ironclad";
}
```

### 3. 卡牌池使用 CustomCardPoolModel

教程中卡牌池应该继承 `CustomCardPoolModel`：

```csharp
public class FireflyCardPool : CustomCardPoolModel
{
    public override string Title => "firefly";
    
    // 可选：自定义能量图标
    public override string? TextEnergyIconPath => null; // 使用默认
    public override string? BigEnergyIconPath => null;  // 使用默认
    
    public override Color DeckEntryCardColor => new Color("E85D04");
    public override Color ShaderColor => new Color("E85D04");
    public override bool IsColorless => false;
}
```

## 自定义资源的分阶段实现

### 第一阶段：基础可玩（当前目标）
- 使用 PlaceholderCharacterModel
- 使用原版 Ironclad 资源作为占位符
- 确保角色可以进入游戏

### 第二阶段：自定义卡池外观
- 创建 `CustomCardPoolModel` 子类
- 设置 `DeckEntryCardColor` 和 `ShaderColor`
- 可选：自定义能量图标

### 第三阶段：自定义角色形象
- 创建 `test_character.tscn` 场景文件
- 设置 `CustomVisualPath`

### 第四阶段：完整自定义
- 逐步添加其他自定义资源
- 能量表盘、休息站形象、商人形象等

## 参考

- 教程文档：`docs/TUTORIAL_04_添加新人物.md`
- PlaceholderCharacterModel 会从未覆盖的属性中自动使用原版资源
