# 知识库 - Slay the Spire 2 Modding

## 角色制作关键知识

### 1. 角色类继承结构
```
System.Object
  └── MegaCrit.Sts2.Core.Models.AbstractModel
        └── MegaCrit.Sts2.Core.Models.CharacterModel
              └── BaseLib.Abstracts.CustomCharacterModel
                    └── YourCharacter (你的角色类)
```

### 2. 必需覆盖的属性

#### 基础属性
- `NameColor` - 角色名字颜色
- `Gender` - 性别 (影响文本代词)
- `StartingHp` - 初始生命值
- `StartingGold` - 初始金币
- `CardPool` - 卡牌池
- `RelicPool` - 遗物池
- `PotionPool` - 药水池
- `StartingDeck` - 起始牌组
- `StartingRelics` - 起始遗物

#### 资源路径（避免黑屏的关键）
必须覆盖以下路径，使用现有角色资源作为占位符：

```csharp
// 在角色构造函数或属性中设置
public override string CustomCharacterSelectIconPath => "res://images/packed/character_select/char_select_ironclad.png";
public override string CustomCharacterSelectLockedIconPath => "res://images/packed/character_select/char_select_ironclad_locked.png";
public override string CustomCharacterSelectBg => "res://scenes/screens/char_select/char_select_bg_ironclad.tscn";
public override string CustomCharacterSelectTransitionPath => "res://materials/transitions/ironclad_transition_mat.tres";
public override string CustomIconPath => "res://images/ui/top_panel/character_icon_ironclad.png";
public override string CustomIconTexturePath => "res://images/ui/top_panel/character_icon_ironclad.png";
public override string CustomVisualPath => "res://scenes/creature_visuals/ironclad.tscn";
public override string CustomEnergyCounterPath => "res://scenes/combat/energy_counters/red_energy_counter.tscn";
public override string CustomRestSiteAnimPath => "res://scenes/rest_site/characters/ironclad_rest_site.tscn";
public override string CustomMerchantAnimPath => "res://scenes/merchant/characters/ironclad_merchant.tscn";
public override string CustomMapMarkerPath => "res://images/packed/map/icons/map_marker_ironclad.png";
public override string CustomTrailPath => "res://scenes/vfx/card_trail_red.tscn";
```

### 3. 注册角色

使用 `ModelDbCustomCharacters.Register`：

```csharp
var baseLibAssembly = AppDomain.CurrentDomain.GetAssemblies()
    .First(a => a.GetName().Name == "BaseLib");
var modelDbCustom = baseLibAssembly.GetType("BaseLib.Abstracts.ModelDbCustomCharacters");
var registerMethod = modelDbCustom.GetMethod("Register", BindingFlags.Public | BindingFlags.Static);
registerMethod.Invoke(null, new[] { new YourCharacter() });
```

---

## 卡牌制作关键知识

### 1. 卡牌类结构
```csharp
[Pool(typeof(YourCardPool))]
public class YourCard : CardModel
{
    public YourCard() : base(cost, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, false)
    {
    }
    
    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new DamageVar(9m, ValueProp.Move)
    };
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 卡牌效果实现
    }
}
```

### 2. [Pool] 特性
BaseLib 0.2.8+ 要求所有模型类标记 `[Pool]` 特性：
- 卡牌: `[Pool(typeof(YourCardPool))]`
- 遗物: `[Pool(typeof(YourRelicPool))]`
- 角色/池类/能力: 不需要 `[Pool]`

### 3. 注册卡牌
```csharp
var contentDictType = baseLibAssembly.GetType("BaseLib.Patches.Content.CustomContentDictionary");
var addModelMethod = contentDictType.GetMethod("AddModel", BindingFlags.Public | BindingFlags.Static);
addModelMethod.Invoke(null, new[] { typeof(YourCard) });
```

---

## 遗物制作关键知识

### 1. 遗物类结构
```csharp
[Pool(typeof(YourRelicPool))]
public class YourRelic : RelicModel
{
    public override RelicRarity Rarity => RelicRarity.Common;
}
```

### 2. 遗物稀有度
- `Common` - 普通（白色）
- `Uncommon` - 罕见（绿色）
- `Rare` - 稀有（蓝色）
- `Boss` - Boss（金色）
- `Special` - 特殊

---

## 本地化关键知识

### 1. Godot 翻译系统

```csharp
// 创建翻译
var translation = new Translation();
translation.Locale = "zh_CN"; // 或 "en"

// 添加消息
translation.AddMessage("characters.YOUR-ID.title", "角色名称");
translation.AddMessage("characters.YOUR-ID.description", "角色描述");
translation.AddMessage("cards.YOUR-ID:name.name", "卡牌名称");
translation.AddMessage("relics.RELIC_ID.title", "遗物名称");

// 注册到翻译服务器
TranslationServer.AddTranslation(translation);
```

### 2. 本地化键格式

| 类型 | 键格式 | 示例 |
|-----|-------|------|
| 角色标题 | `characters.{ID}.title` | `characters.MYSTS2MOD-FIREFLY.title` |
| 角色描述 | `characters.{ID}.description` | `characters.MYSTS2MOD-FIREFLY.description` |
| 卡牌名称 | `cards.{ID}:name.name` | `cards.MYSTS2MOD:FIREFLYSTRIKE.name` |
| 卡牌描述 | `cards.{ID}:name.description` | `cards.MYSTS2MOD:FIREFLYSTRIKE.description` |
| 遗物名称 | `relics.{ID}.title` | `relics.SAM_ARMOR.title` |
| 遗物描述 | `relics.{ID}.description` | `relics.SAM_ARMOR.description` |

---

## 项目配置关键知识

### 1. .csproj 关键配置

```xml
<Project Sdk="Godot.NET.Sdk/4.5.1">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <EnableDynamicLoading>true</EnableDynamicLoading>
    <AssemblyName>YourModName</AssemblyName>  <!-- DLL 文件名 -->
  </PropertyGroup>
  
  <!-- BaseLib NuGet 包 -->
  <ItemGroup>
    <PackageReference Include="Alchyr.Sts2.BaseLib" Version="*" />
  </ItemGroup>
  
  <!-- 游戏 DLL 引用 -->
  <ItemGroup>
    <Reference Include="sts2">
      <HintPath>$(GameDataDir)\sts2.dll</HintPath>
      <Private>false</Private>
    </Reference>
  </ItemGroup>
</Project>
```

### 2. Mod Manifest (modname.json)

```json
{
    "id": "YourModName",
    "name": "显示名称",
    "author": "作者名",
    "description": "Mod 描述",
    "version": "1.0.0",
    "has_pck": false,
    "has_dll": true,
    "dependencies": ["BaseLib"],
    "affects_gameplay": true
}
```

---

## 常见问题 FAQ

### Q: 角色显示但无法开始游戏（黑屏）？
**A**: 缺少资源路径覆盖。确保在 `CustomCharacterModel` 子类中覆盖所有 `Custom*` 资源路径属性。

### Q: 卡牌/遗物没有名称显示？
**A**: 缺少本地化。使用 `LocalizationHelper` 或 `TranslationServer.AddTranslation` 添加本地化文本。

### Q: Mod 没有被加载？
**A**: 检查以下几点：
1. `modname.json` 中的 `id` 必须与 DLL 文件名匹配
2. DLL 文件必须放在 `mods/YourModName/` 目录下
3. `mods_config.json` 中必须启用该 mod

### Q: BaseLib 报错 "must be marked with a PoolAttribute"？
**A**: 给卡牌和遗物类添加 `[Pool(typeof(YourPool))]` 特性。

### Q: 如何调试？
**A**: 查看游戏日志：`%APPDATA%\SlayTheSpire2\logs\godot.log`
