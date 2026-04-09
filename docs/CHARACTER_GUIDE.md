# 杀戮尖塔2 新角色制作指南

## 概览

制作新角色需要以下内容：

1. **角色类** - 继承 `CharacterModel`
2. **初始卡组** - 10张卡牌（通常）
3. **初始遗物** - 1个专属遗物
4. **专属卡牌** - 角色独特的卡牌池
5. **角色美术** - 头像、按钮、Spine动画
6. **本地化文本** - 描述、对话等

---

## 角色类结构

```csharp
public class MyCharacter : CharacterModel
{
    public MyCharacter()
    {
        // 1. 基础信息
        Id = "ModId:CharacterName";
        Name = "角色名";
        Description = "角色描述";

        // 2. 初始数值
        MaxHealth = 70;        // 最大生命值
        StartingHealth = 70;   // 起始生命值
        StartingGold = 99;     // 起始金币

        // 3. 角色颜色 (影响卡牌颜色、UI主题)
        Color = new Color(r, g, b);

        // 4. 资源路径
        PortraitPath = "ModId/characters/portrait.png";
        ButtonPath = "ModId/characters/button.png";
        SkeletonPath = "ModId/characters/character.skel";
        AtlasPath = "ModId/characters/character.atlas";

        // 5. 初始内容
        InitializeDeck();
        InitializeRelics();
    }
}
```

---

## 初始卡组设计

标准初始卡组包含10张牌：

| 数量 | 类型 | 说明 |
|------|------|------|
| 4张 | 基础攻击 | 造成6-8点伤害 |
| 4张 | 基础防御 | 获得5-8点格挡 |
| 1张 | 专属攻击 | 体现角色机制 |
| 1张 | 专属技能 | 体现角色机制 |

### 示例

```csharp
private void InitializeDeck()
{
    // 4张基础打击
    for (int i = 0; i < 4; i++)
        StartingDeck.Add(new CustomStrike());

    // 4张基础防御
    for (int i = 0; i < 4; i++)
        StartingDeck.Add(new CustomDefend());

    // 2张专属卡
    StartingDeck.Add(new SignatureCard1());
    StartingDeck.Add(new SignatureCard2());
}
```

---

## 卡牌颜色设置

角色专属卡牌应该使用角色颜色：

```csharp
public class CustomCard : CardModel
{
    public CustomCard()
    {
        // 设置为自定义颜色
        CardColor = CardColor.Custom;
        CustomColor = new Color(0.4f, 0.1f, 0.6f); // 你的角色颜色
    }
}
```

---

## 角色机制设计

### 常见机制类型

1. **资源系统** - 如战士的怒气、猎人的连击点
2. **状态叠加** - 如毒、虚弱、易伤
3. **特殊机制** - 如格挡保留、生命回复、抽牌等

### 示例：闪避机制

```csharp
/// <summary>
/// 闪避能力 - 抵消下一次受到的攻击
/// </summary>
public class DodgePower : PowerModel
{
    public DodgePower()
    {
        Name = "闪避";
        Description = "抵消下一次受到的攻击。";
        Type = PowerType.Buff;
    }

    // 修改受到的伤害
    public override float ModifyIncomingDamage(float damage, CombatContext ctx)
    {
        if (damage > 0 && Amount > 0)
        {
            Amount--;  // 消耗一层
            return 0;   // 抵消伤害
        }
        return damage;
    }
}
```

---

## Spine 动画制作

### 规格要求

- **格式**: Spine 4.2 二进制 (.skel)
- **缩放**: 约需10倍缩放（相比1代）
- **动画轴**: 6种内置动画
  - `idle` - 待机
  - `attack` - 攻击
  - `hit` - 受击
  - `victory` - 胜利
  - `defeat` - 失败
  - `evade` - 闪避

### 从1代迁移

如果你有1代的Spine动画：
1. 使用 SpineSkeletonDataConverter 转换
2. 修改版本字符串（最低支持3.5）
3. 重新导出为4.2二进制格式
4. 调整缩放比例（约10倍）

---

## 美术资源清单

### 必须资源

| 资源 | 尺寸建议 | 格式 |
|------|----------|------|
| 角色头像 | 256x256 | PNG |
| 选择按钮 | 512x256 | PNG |
| Spine动画 | - | .skel + .atlas |
| 基础攻击卡 | 250x190 | PNG |
| 基础防御卡 | 250x190 | PNG |
| 专属卡牌 | 250x190 | PNG |
| 初始遗物 | 128x128 | PNG |
| 能力图标 | 64x64 | PNG |

### 资源路径

```
assets/
├── characters/
│   ├── shadow_dancer_portrait.png
│   ├── shadow_dancer_button.png
│   ├── shadow_dancer.skel
│   └── shadow_dancer.atlas
├── cards/
│   ├── shadow_strike.png
│   ├── shadow_defend.png
│   ├── shadow_step.png
│   └── shadow_cloak.png
├── relics/
│   ├── shadow_amulet.png
│   └── shadow_amulet_outline.png
└── powers/
    └── dodge.png
```

---

## 使用 BaseLib 简化开发

如果使用 BaseLib 框架，可以简化角色制作：

```csharp
using Alchyr.Sts2.BaseLib.Characters;

public class ShadowDancer : CustomCharacterModel
{
    public ShadowDancer()
    {
        Info = new CharacterInfo {
            Name = "影舞者",
            MaxHealth = 70,
            Color = new Color(0.4f, 0.1f, 0.6f)
        };

        // 自动注册
        AddStarterCard<ShadowStrike>(4);
        AddStarterCard<ShadowDefend>(4);
        AddStarterCard<ShadowStep>(1);
        AddStarterCard<ShadowCloak>(1);

        AddStarterRelic<ShadowAmulet>();
    }
}
```

---

## 调试技巧

### 1. 检查角色是否正确注册

```csharp
[ModInitializer("Initialize")]
public static void Initialize()
{
    var character = new ShadowDancer();
    Log.Info($"角色: {character.Name}");
    Log.Info($"初始卡组数量: {character.StartingDeck.Count}");
    Log.Info($"初始遗物数量: {character.StartingRelics.Count}");

    CharacterRegistry.Register(character);
}
```

### 2. 常见问题

| 问题 | 解决方案 |
|------|----------|
| 角色不显示 | 检查资源路径是否正确 |
| 动画不播放 | 确保Spine版本是4.2二进制格式 |
| 卡牌无颜色 | 设置 `CardColor.Custom` 和 `CustomColor` |
| 遗物不生效 | 检查遗物是否正确添加到 `StartingRelics` |

---

## 完整示例

查看项目中的完整示例：
- `src/characters/ExampleCharacter.cs` - 影舞者角色完整实现

---

## 下一步

1. **设计角色机制** - 确定核心玩法
2. **创建美术资源** - 或使用占位图测试
3. **实现卡牌** - 从基础卡牌开始
4. **测试平衡性** - 多次游戏测试

需要我详细解释某个部分吗？比如：
- Spine动画制作流程
- 特定机制实现（如连击、充能等）
- 卡牌平衡性设计
