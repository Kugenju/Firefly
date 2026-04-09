# Firefly 流萤角色 Mod

> 杀戮尖塔2 (Slay the Spire 2) 角色 Mod - 添加来自《崩坏：星穹铁道》的流萤角色

## 项目概述

这是一个基于 BaseLib 框架开发的 Slay the Spire 2 角色 Mod，添加了「流萤」作为可玩角色。

**技术栈**:
- Godot 4.5.1 + C# / .NET 9.0
- BaseLib Modding Framework

**已实现内容**:
- 13 张专属卡牌（攻击、技能、能力）
- 6 个专属遗物
- 完整的角色框架（继承 PlaceholderCharacterModel）
- 中文本地化支持

---

## 项目结构

```
Firefly/
├── Firefly.json                    # Mod 清单文件
├── firefly.csproj                  # C# 项目文件
├── project.godot                   # Godot 项目配置
├── export_presets.cfg              # 导出配置
├── Scripts/                        # C# 源代码
│   ├── Entry.cs                    # Mod 入口点
│   ├── Characters/
│   │   └── Firefly.cs              # 角色定义
│   ├── Cards/                      # 13 张卡牌
│   ├── Relics/                     # 6 个遗物
│   ├── CardPools/
│   │   └── FireflyCardPool.cs      # 卡牌池
│   ├── RelicPools/
│   │   └── FireflyRelicPool.cs     # 遗物池
│   └── PotionPools/
│       └── FireflyPotionPool.cs    # 药水池
├── Firefly/                        # 【重要】资源文件夹
│   ├── localization/zhs/           # 中文本地化
│   │   ├── cards.json
│   │   ├── characters.json
│   │   ├── relics.json
│   │   └── keywords.json
│   └── images/cards/               # 卡牌图片（未来使用）
└── docs/                           # 文档
    ├── README.md                   # 本文件
    ├── LOCALIZATION_AND_VARS.md    # 本地化与变量指南
    └── TUTORIAL_04_添加新人物.md   # 角色制作详细教程
```

**关键规则**: `Firefly/` 子文件夹名必须与 `Firefly.json` 中的 `id` 字段一致。

---

## 快速开始

### 环境要求

| 工具 | 版本 | 说明 |
|------|------|------|
| .NET SDK | 9.0+ | 编译 C# 代码 |
| Godot | 4.5.1 .NET | 导出 PCK 资源包 |
| 杀戮尖塔2 | Steam 版 | 游戏本体 |

> ⚠️ **必须使用 Godot 4.5.1**，其他版本打包的 .pck 会被游戏拒绝！

### 配置步骤

1. **修改 `firefly.csproj` 中的游戏路径**

```xml
<!-- 修改为你的实际游戏路径 -->
<Sts2Dir>G:​\SteamLibrary\steamapps\common\Slay the Spire 2</Sts2Dir>
<GodotExe>F:​\Godot\Godot_v4.5.1-stable_mono_win64\Godot_v4.5.1-stable_mono_win64\Godot_v4.5.1-stable_mono_win64.exe</GodotExe>
```

2. **构建并安装**

```powershell
# 编译 DLL（自动复制到游戏目录）
dotnet build firefly.csproj

# 导出 PCK 资源包（包含本地化文件）
dotnet build firefly.csproj -t:ExportPck
```

3. **启动游戏测试**

构建完成后，Mod 文件将自动安装到：
```
Slay the Spire 2/mods/Firefly/
├── firefly.dll       # 编译的代码
├── Firefly.json      # Mod 清单
└── Firefly.pck       # 资源包（包含本地化）
```

---

## 当前实现

### 角色定义

```csharp
public sealed class Firefly : PlaceholderCharacterModel
{
    public const string CharacterId = "firefly";

    // 基础属性
    public override Color NameColor => new Color("E85D04");
    public override Color EnergyLabelOutlineColor => new Color("D00000");
    public override CharacterGender Gender => CharacterGender.Feminine;
    public override int StartingHp => 70;
    public override int StartingGold => 99;

    // 池子配置
    public override CardPoolModel CardPool => ModelDb.CardPool<FireflyCardPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<FireflyPotionPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<FireflyRelicPool>();

    // 初始牌组（10张）
    public override IEnumerable<CardModel> StartingDeck => new CardModel[]
    {
        ModelDb.Card<FireflyStrike>(),  // x4
        ModelDb.Card<FireflyDefend>(),  // x4
        ModelDb.Card<ChrysalidPyronexus>(),
        ModelDb.Card<MeteoricIncineration>(),
    };

    // 初始遗物
    public override IReadOnlyList<RelicModel> StartingRelics => new[]
    {
        ModelDb.Relic<SamArmor>()
    };
}
```

### 卡牌列表（13张）

| 卡牌 | 类型 | 稀有度 | 费用 | 效果 |
|------|------|--------|------|------|
| 流萤打击 | 攻击 | 基础 | 1 | 造成 6/9 点伤害 |
| 流萤防御 | 技能 | 基础 | 1 | 获得 5/8 点格挡 |
| 茧中薪火 | 攻击 | 罕见 | 1 | 造成 8/11 点伤害 |
| 陨落流星 | 攻击 | 罕见 | 2 | 对所有敌人造成 8/12 点伤害 |
| 完全燃烧 | 技能 | 稀有 | 3 | 进入完全燃烧状态 |
| 余烬之刃 | 攻击 | 普通 | 1 | 造成 9/12 点伤害 |
| 烈焰鞭挞 | 攻击 | 普通 | 1 | 造成 7/9 点伤害两次 |
| 热能护盾 | 技能 | 普通 | 1 | 获得 8/11 点格挡 |
| 点燃冲刺 | 攻击 | 普通 | 1 | 造成 8/11 点伤害 |
| 超新星爆发 | 攻击 | 稀有 | 2 | 造成 20/28 点伤害 |
| 等离子囚笼 | 技能 | 稀有 | 2 | 给予所有敌人脆弱和虚弱 |
| 烈焰吞噬 | 攻击 | 罕见 | 2 | 消耗燃烧层数造成伤害 |
| 燃烧护盾 | 技能 | 罕见 | 2 | 获得 15/20 点格挡 |

### 卡牌代码示例

```csharp
[Pool(typeof(FireflyCardPool))]
public class FireflyStrike : CardModel
{
    public FireflyStrike() : base(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy, false)
    {
    }

    // 【关键】定义描述中使用的变量
    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new DamageVar(6m, ValueProp.Move)  // 对应 {Damage:diff()}
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target != null)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .Execute(choiceContext);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);  // 升级后 6 -> 9
    }
}
```

### 本地化示例

```json
// Firefly/localization/zhs/cards.json
{
    "FIREFLY_STRIKE.title": "流萤打击",
    "FIREFLY_STRIKE.description": "造成{Damage:diff()}点伤害。",
    "FIREFLY_DEFEND.title": "流萤防御",
    "FIREFLY_DEFEND.description": "获得{Block:diff()}点格挡。"
}
```

**变量命名规则**:
- 卡牌 ID: 类名转 snake_case（`FireflyStrike` → `FIREFLY_STRIKE`）
- 变量: `{Damage:diff()}`, `{Block:diff()}` 等

详见 [LOCALIZATION_AND_VARS.md](./LOCALIZATION_AND_VARS.md)

---

## 开发指南

### 1. 卡牌开发

创建新卡牌的步骤：

1. 在 `Scripts/Cards/` 创建类，继承 `CardModel`
2. 添加 `[Pool(typeof(FireflyCardPool))]` 特性
3. 定义 `CanonicalVars`（用于描述中的变量）
4. 实现 `OnPlay` 方法
5. 在 `Firefly/localization/zhs/cards.json` 添加本地化
6. 重新构建并导出 PCK

### 2. 角色资源

当前使用 `PlaceholderCharacterModel`，未设置的路径会自动使用原版铁甲战士资源。

如需自定义资源，覆盖对应属性：
```csharp
public override string CustomVisualPath => "res://Firefly/scenes/character.tscn";
```

详见 [TUTORIAL_04_添加新人物.md](./TUTORIAL_04_添加新人物.md)

### 3. 调试

游戏日志位置：
```
%APPDATA%/SlayTheSpire2/logs/godot.log
```

常用 Steam 启动参数：
- `--nomods` - 禁用所有 Mod（排查崩溃用）
- `--seed 12345` - 指定随机种子

---

## 参考资源

- [BaseLib-StS2](https://github.com/Alchyr/BaseLib-StS2) - Mod 开发框架
- [STS2 Modding Tutorials](https://github.com/GlitchedReme/SlayTheSpireModTutorials) - 中文教程

---

## 许可证

本项目仅供学习交流使用。
- 杀戮尖塔2 版权归 MegaCrit 所有
- 流萤角色版权归 HoYoverse/米哈游 所有
