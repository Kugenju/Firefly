# 04 添加新人物

> 来源: https://glitchedreme.github.io/SlayTheSpire2ModdingTutorials/
> 整理时间: 2026年4月9日

## 依赖

需要添加对 BaseLib 的依赖，可以省去很多功夫。

## 创建池子

需要创建人物独有的卡牌、药水、遗物池各一个。

### TestCardPool.cs

```csharp
public class TestCardPool : CustomCardPoolModel
{
    // 卡池的ID。必须唯一防撞车。
    public override string Title => "test";

    // 描述中使用的能量图标。大小为24x24。
    public override string? TextEnergyIconPath => "res://test/images/energy_test.png";
    // tooltip和卡牌左上角的能量图标。大小为74x74。
    public override string? BigEnergyIconPath => "res://test/images/energy_test_big.png";

    // 卡池的主题色。
    public override Color DeckEntryCardColor => new(0.5f, 0.5f, 1f);

    // 如果你使用默认的卡框，可以使用这个颜色来修改卡框的颜色。
    public override Color ShaderColor => new(0.5f, 0.5f, 1f);

    // 如果你使用自定义卡框图片，重写CustomFrame方法并返回你的卡框图片。
    // public override Texture2D? CustomFrame(CustomCardModel card)
    // {
    //     return card.Type switch
    //     {
    //         CardType.Attack => PreloadManager.Cache.GetAsset<Texture2D>("res://test/images/card_frame_attack.png"),
    //         CardType.Power => PreloadManager.Cache.GetAsset<Texture2D>("res://test/images/card_frame_power.png"),
    //         _ => PreloadManager.Cache.GetAsset<Texture2D>("res://test/images/card_frame_skill.png"),
    //     };
    // }
    
    // 卡池是否是无色。例如事件、状态等卡池就是无色的。
    public override bool IsColorless => false;
}
```

### TestRelicPool.cs

```csharp
public class TestRelicPool : CustomRelicPoolModel
{
    // 描述中使用的能量图标。大小为24x24。
    public override string? TextEnergyIconPath => "res://test/images/energy_test.png";
    // tooltip和卡牌左上角的能量图标。大小为74x74。
    public override string? BigEnergyIconPath => "res://test/images/energy_test_big.png";
}
```

### TestPotionPool.cs

```csharp
public class TestPotionPool : CustomPotionPoolModel
{
    // 描述中使用的能量图标。大小为24x24。
    public override string? TextEnergyIconPath => "res://test/images/energy_test.png";
    // tooltip和卡牌左上角的能量图标。大小为74x74。
    public override string? BigEnergyIconPath => "res://test/images/energy_test_big.png";
}
```

### 修改卡牌池归属

当你创建你自己人物的池子时，不要忘了把你的卡牌药水遗物等（比如打击）的Pool改成你的池子：

```csharp
// 加入哪个卡池
[Pool(typeof(TestCardPool))]
public class TestCard : CustomCardModel
```

## 创建人物

人物需要极其大量的资源，**推荐新建类继承 PlaceholderCharacterModel 而不是 CustomCharacterModel**。你没有的资源直接注释掉以使用原版。

### TestCharacter.cs

```csharp
public class TestCharacter : PlaceholderCharacterModel
{
    // 角色名称颜色
    public override Color NameColor => new(0.5f, 0.5f, 1f);
    // 能量图标轮廓颜色
    public override Color EnergyLabelOutlineColor => new(0.1f, 0.1f, 1f);

    // 人物性别（男女中立）
    public override CharacterGender Gender => CharacterGender.Masculine;

    // 初始血量
    public override int StartingHp => 80;

    // 人物模型tscn路径。要自定义见下。
    public override string CustomVisualPath => "res://test/scenes/test_character.tscn";
    // 卡牌拖尾场景。
    // public override string CustomTrailPath => "res://scenes/vfx/card_trail_ironclad.tscn";
    // 人物头像路径。
    public override string CustomIconTexturePath => "res://icon.svg";
    // 人物头像2号。
    // public override string CustomIconPath => "res://scenes/ui/character_icons/ironclad_icon.tscn";
    // 能量表盘tscn路径。要自定义见下。
    public override string CustomEnergyCounterPath => "res://test/scenes/test_energy_counter.tscn";
    // 篝火休息场景。
    // public override string CustomRestSiteAnimPath => "res://scenes/rest_site/characters/ironclad_rest_site.tscn";
    // 商店人物场景。
    // public override string CustomMerchantAnimPath => "res://scenes/merchant/characters/ironclad_merchant.tscn";
    // 多人模式-手指。
    // public override string CustomArmPointingTexturePath => null;
    // 多人模式剪刀石头布-石头。
    // public override string CustomArmRockTexturePath => null;
    // 多人模式剪刀石头布-布。
    // public override string CustomArmPaperTexturePath => null;
    // 多人模式剪刀石头布-剪刀。
    // public override string CustomArmScissorsTexturePath => null;

    // 人物选择背景。
    public override string CustomCharacterSelectBg => "res://test/scenes/test_bg.tscn";
    // 人物选择图标。
    public override string CustomCharacterSelectIconPath => "res://test/images/char_select_test.png";
    // 人物选择图标-锁定状态。
    public override string CustomCharacterSelectLockedIconPath => "res://test/images/char_select_test_locked.png";
    // 人物选择过渡动画。
    // public override string CustomCharacterSelectTransitionPath => "res://materials/transitions/ironclad_transition_mat.tres";
    // 地图上的角色标记图标、表情轮盘上的角色头像
    // public override string CustomMapMarkerPath => null;
    // 攻击音效
    // public override string CustomAttackSfx => null;
    // 施法音效
    // public override string CustomCastSfx => null;
    // 死亡音效
    // public override string CustomDeathSfx => null;
    // 角色选择音效
    // public override string CharacterSelectSfx => null;
    // 过渡音效。这个不能删。
    public override string CharacterTransitionSfx => "event:/sfx/ui/wipe_ironclad";

    public override CardPoolModel CardPool => ModelDb.CardPool<TestCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<TestRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<TestPotionPool>();

    // 初始卡组
    public override IEnumerable<CardModel> StartingDeck => [
        ModelDb.Card<TestCard>(),
        ModelDb.Card<TestCard>(),
        ModelDb.Card<TestCard>(),
        ModelDb.Card<TestCard>(),
        ModelDb.Card<TestCard>(),
    ];

    // 初始遗物
    public override IReadOnlyList<RelicModel> StartingRelics => [
        ModelDb.Relic<TestRelic>(),
    ];

    // 攻击建筑师的攻击特效列表
    public override List<string> GetArchitectAttackVfx() => [
        "vfx/vfx_attack_blunt",
        "vfx/vfx_heavy_blunt",
        "vfx/vfx_attack_slash",
        "vfx/vfx_bloody_impact",
        "vfx/vfx_rock_shatter"
    ];
}
```

## 自定义人物背景

```csharp
public override string CustomCharacterSelectBg => "res://test/scenes/test_bg.tscn";
```

没什么要求，Godot里创建一个新的场景，类型为Control，自己搭建场景即可。

## 自定义人物

```csharp
public override string CustomVisualPath => "res://test/scenes/test_character.tscn";
```

新建一个Node2D类型的场景，如下结构：

```
TestCharacter (Node2D)
├── Visuals (Node2D) %
├── Bounds (Control) %
├── IntentPos (Marker2D) %
└── CenterPos (Marker2D) %
```

其中 Visuals，Bounds，IntentPos，CenterPos 需要右键勾选**作为唯一名称访问**，出现`%`即可。名字不要改。

- **Bounds** 就是你的人物 hitbox 的大小，如果你觉得血条太短调整一下它的大小。
- 人物显示在 x 轴上方。

### 使用3D模型

如果想使用3d模型，新建 `visuals→subviewportcontainer→subviewport` 的层级结构，然后在 subviewport 中添加 camera3d 和任意3d模型，在3d视图中调整视角至2d视图正常显示。最后设置 subviewport 的 transparent 为 true。

### 人物动画

其中 Visuals 可以更改成任意继承了 Node2D 的类型，例如 SpineSprite，Sprite2D，AnimatedSprite2D 或是 AnimationPlayer，或者在它之下新建节点都可。

- 如果要自然支持 Spine 播放，需要把 Visuals 改成 SpineSprite，且你的战斗人物模型需要有 `idle_loop`（待机循环）、`attack`（攻击动作）、`cast`（能力卡动作）、`hurt`（受伤）、`die`（死亡）这些动画名。
- 如果你只有一张图，那么把 Visuals 改成 Sprite2D 类型更改图片即可。
- 如果你使用 AnimatedSprite2D，确保动画名和上方一致。

此外 BaseLib 支持使用 AnimationPlayer 控制动画。虽然 AnimationPlayer 放在任意位置都可以，但推荐把根节点之下。动画名和上方设置的一致即可自动播放动画。

## 自定义能量表盘

```csharp
public override string CustomEnergyCounterPath => "res://test/scenes/test_energy_counter.tscn";
```

建议从原版或者下面的附赠资源处复制一份tscn快速开始。

创建一个 Control 类型的新场景，设定以下结构：

```
TestEnergyCounter (Control)
├── EnergyVfxBack (Node2D) %
├── Layers (Control) %
│   ├── Layer1 (TextureRect，或任意)
│   └── RotationLayers (Control) %
├── EnergyVfxFront (Node2D) %
└── Label (Label)
```

后面标 `%` 的需要作为唯一名称访问。名字不要改，label 也是。

**RotationLayers** 里放需要旋转的图层。没有也行。

由于 BaseLib 做了工作，你的节点现在不需要挂载脚本了。

## 自定义商店模型

```csharp
public override string CustomMerchantAnimPath => "res://test/scenes/test_character_merchant.tscn";
```

创建一个 Node2D 类型的新场景。

- 如果你使用 Spine 模型，第一个子节点放置 SpineSprite，且动画名是 `relaxed_loop`。
- 如果你使用其他动画，创建一个继承了 `NMerchantCharacter` 的节点，并在 `_ready` 函数里播放你自己的动画。
- 静态图就不需要了。

```csharp
using MegaCrit.Sts2.Core.Nodes.Screens.Shops;

namespace Test.Scripts;

public partial class TestCharacterMerchant : NMerchantCharacter
{
    public override void _Ready() { }
}
```

## 本地化文件

创建 `{modId}/localization/{Language}/characters.json`，填写以下内容：

```json
{
  "TEST-TEST_CHARACTER.aromaPrinciple": "[sine][blue]……等待……[/blue][/sine]",
  "TEST-TEST_CHARACTER.banter.alive.endTurnPing": "……",
  "TEST-TEST_CHARACTER.banter.dead.endTurnPing": "……",
  "TEST-TEST_CHARACTER.cardsModifierDescription": "戈多的卡牌现在会出现在奖励和商店中。",
  "TEST-TEST_CHARACTER.cardsModifierTitle": "戈多卡牌",
  "TEST-TEST_CHARACTER.description": "一个在无尽等待中的存在。\n时间对[gold]戈多[/gold]而言，不过是另一种形式的永恒。",
  "TEST-TEST_CHARACTER.eventDeathPrevention": "我还得继续等下去……",
  "TEST-TEST_CHARACTER.goldMonologue": "[sine]这些金币……也许能派上用场……[/sine]",
  "TEST-TEST_CHARACTER.possessiveAdjective": "他的",
  "TEST-TEST_CHARACTER.pronounObject": "他",
  "TEST-TEST_CHARACTER.pronounPossessive": "他的",
  "TEST-TEST_CHARACTER.pronounSubject": "他",
  "TEST-TEST_CHARACTER.title": "戈多",
  "TEST-TEST_CHARACTER.titleObject": "戈多",
  "TEST-TEST_CHARACTER.unlockText": "用[pink]{Prerequisite}[/pink]进行一局游戏来解锁这个角色。"
}
```

### 先古对话

同时还需要先古对话的 json。创建 `{modId}/localization/{Language}/ancients.json`。

不要忘记在你的 Init 初始化函数中添加：

```csharp
ScriptManagerBridge.LookupScriptsInAssembly(typeof(Entry).Assembly);
```

打开项目 → 项目设置，把**将文本资源转换为二进制**禁用。

## 关键要点总结

1. **使用 PlaceholderCharacterModel** - 没有的资源会自动使用原版，避免黑屏
2. **所有资源路径都是可选的** - 不需要的资源直接注释掉或设为 null
3. **必须覆盖的属性**:
   - `NameColor` - 角色名称颜色
   - `EnergyLabelOutlineColor` - 能量轮廓颜色
   - `Gender` - 性别
   - `StartingHp` - 初始血量
   - `CardPool`/`RelicPool`/`PotionPool` - 池子
   - `StartingDeck` - 初始卡组
   - `StartingRelics` - 初始遗物
   - `CharacterTransitionSfx` - 过渡音效（不能删）

4. **场景文件命名规范** - 所有节点名和唯一名称访问标记 (`%`) 必须严格按照教程设置
