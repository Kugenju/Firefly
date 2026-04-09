# 杀戮尖塔2 Mod 开发 - 本地化与变量指南

## 1. 项目结构

```
Firefly (项目根目录)
├── Firefly.json              # Mod 清单文件
├── firefly.csproj            # 项目配置文件
├── Scripts/                  # C# 代码
└── Firefly/                  # 【重要】modid 命名的资源文件夹
    ├── images/
    │   └── cards/            # 卡牌图片
    └── localization/         # 本地化文件
        └── zhs/              # 简体中文
            ├── cards.json
            ├── characters.json
            ├── relics.json
            └── keywords.json
```

**关键规则**: `Firefly/` 子文件夹名必须与 `Firefly.json` 中的 `id` 字段一致。

---

## 2. 本地化 Key 命名规则

### 2.1 卡牌 (cards.json)

BaseLib 会自动将类名转换为 snake_case 作为 ID。

```csharp
// C# 类名
public class FireflyStrike : CardModel { }
public class ChrysalidPyronexus : CardModel { }
```

```json
// 对应的本地化 key（类名转 snake_case）
{
    "FIREFLY_STRIKE.title": "流萤打击",
    "FIREFLY_STRIKE.description": "造成{Damage:diff()}点伤害。",
    
    "CHRYSALID_PYRONEXUS.title": "茧中薪火",
    "CHRYSALID_PYRONEXUS.description": "造成{Damage:diff()}点伤害。"
}
```

**命名转换规则**:
- `FireflyStrike` → `FIREFLY_STRIKE`
- `ChrysalidPyronexus` → `CHRYSALID_PYRONEXUS`
- `CompleteCombustionCard` → `COMPLETE_COMBUSTION_CARD`

### 2.2 角色 (characters.json)

角色使用带 Mod 前缀的格式:

```json
{
    "FIREFLY.title": "流萤",
    "FIREFLY.titleObject": "流萤",
    "FIREFLY.description": "角色描述...",
    "FIREFLY.possessiveAdjective": "她的",
    "FIREFLY.pronounObject": "她",
    "FIREFLY.pronounPossessive": "她的",
    "FIREFLY.pronounSubject": "她",
    
    // 兼容格式（某些情况下游戏会查找带前缀的版本）
    "FIREFLY-FIREFLY.title": "流萤",
    "FIREFLY-FIREFLY.titleObject": "流萤"
}
```

### 2.3 遗物 (relics.json)

```json
{
    "SAM_ARMOR.title": "萨姆装甲",
    "SAM_ARMOR.description": "遗物描述...",
    
    "COMBUSTION_ENGINE.title": "完全燃烧引擎",
    "COMBUSTION_ENGINE.description": "每当你进入完全燃烧状态时..."
}
```

---

## 3. 描述变量 (DynamicVar)

### 3.1 基础变量

| 变量名 | 对应类 | 用途 | 代码定义 | 描述写法 |
|--------|--------|------|----------|----------|
| `Damage` | `DamageVar` | 伤害 | `new DamageVar(6m, ValueProp.Move)` | `{Damage:diff()}` |
| `Block` | `BlockVar` | 格挡 | `new BlockVar(5m, ValueProp.Move)` | `{Block:diff()}` |
| `MagicNumber` | `MagicNumberVar` | 魔法数字（通用） | `new MagicNumberVar(2m, ValueProp.Move)` | `{MagicNumber:diff()}` |
| `Cards` | `CardsVar` | 卡牌数量 | `new CardsVar(2m, ValueProp.Move)` | `{Cards:diff()}` |
| `Energy` | `EnergyVar` | 能量 | `new EnergyVar(1m, ValueProp.Move)` | `{Energy:energyIcons()}` |
| `Heal` | `HealVar` | 治疗 | `new HealVar(5m, ValueProp.Move)` | `{Heal:diff()}` |
| `Repeat` | `RepeatVar` | 重复次数 | `new RepeatVar(2m, ValueProp.Move)` | `{Repeat:diff()}` |

### 3.2 状态变量（能力层数）

| 变量名 | 对应类 | 用途 |
|--------|--------|------|
| `StrengthPower` | `PowerVar<StrengthPower>` | 力量层数 |
| `DexterityPower` | `PowerVar<DexterityPower>` | 敏捷层数 |
| `WeakPower` | `PowerVar<WeakPower>` | 虚弱层数 |
| `VulnerablePower` | `PowerVar<VulnerablePower>` | 易伤层数 |
| `PoisonPower` | `PowerVar<PoisonPower>` | 中毒层数 |

### 3.3 代码示例

```csharp
public class FireflyStrike : CardModel
{
    public FireflyStrike() : base(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy, false)
    {
    }

    // 【关键】必须定义 CanonicalVars，变量名与描述中的 {} 对应
    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new DamageVar(6m, ValueProp.Move)  // 对应 {Damage:diff()}
    };

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);  // 升级后变为 9
    }
}
```

---

## 4. Formatter 格式化

### 4.1 游戏自定义 Formatter

| Formatter | 效果 | 示例 |
|-----------|------|------|
| `diff()` | 高于基础变绿，低于基础变红 | `{Damage:diff()}` |
| `inverseDiff()` | 高于基础变红，低于基础变绿 | `{HpLoss:inverseDiff()}` |
| `energyIcons()` | 显示能量图标 | `{Energy:energyIcons()}` |
| `starIcons()` | 显示辉星图标 | `{Stars:starIcons()}` |
| `abs()` | 绝对值 | `{Damage:abs()}` |
| `percentMore()` | 百分比增加 | `{Boost:percentMore()}` |
| `percentLess()` | 百分比减少 | `{Reduction:percentLess()}` |

### 4.2 SmartFormat 内置 Formatter

| Formatter | 说明 | 示例 |
|-----------|------|------|
| `cond` | 条件分支 | `{X:cond:>0?生效|不生效}` |
| `choose` | 选择分支 | `{X:choose(1|2):一|{:diff()}}` |
| `plural` | 复数 | `{Cards:plural:card|cards}` |

---

## 5. 完整示例

### 5.1 基础攻击牌

```csharp
// C# 代码
public class FireflyStrike : CardModel
{
    public FireflyStrike() : base(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy, false) { }

    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new DamageVar(6m, ValueProp.Move)
    };

    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(3m);
}
```

```json
// 本地化
{
    "FIREFLY_STRIKE.title": "流萤打击",
    "FIREFLY_STRIKE.description": "造成{Damage:diff()}点伤害。"
}
```

### 5.2 带魔法数字的卡牌

```csharp
// C# 代码
public class ChrysalidPyronexus : CardModel
{
    public ChrysalidPyronexus() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, false) { }

    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new DamageVar(8m, ValueProp.Move),
        new MagicNumberVar(2m, ValueProp.Move)  // 魔法数字，用于燃烧层数
    };

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
        DynamicVars.MagicNumber.UpgradeValueBy(1m);
    }
}
```

```json
// 本地化
{
    "CHRYSALID_PYRONEXUS.title": "茧中薪火",
    "CHRYSALID_PYRONEXUS.description": "造成{Damage:diff()}点伤害。燃烧：获得{MagicNumber:diff()}层燃烧。"
}
```

---

## 6. 常见问题

### Q: 描述显示原始文本如 `{Damage:diff()}` 而不是数字？

**原因**: `CanonicalVars` 未正确定义，或变量名不匹配。

**解决**: 检查代码中是否正确定义了 `CanonicalVars`，且变量名与描述中的 `{}` 一致。

### Q: 本地化 key 找不到？

**原因**: 
1. 使用了错误的前缀格式（如 `FIREFLY-FIREFLY_STRIKE` 而不是 `FIREFLY_STRIKE`）
2. PCK 文件未正确导出或过期

**解决**: 检查日志中的 key 名称，确保与代码生成的 ID 一致。

### Q: 如何查看卡牌/遗物的实际 ID？

**方法**: 查看游戏日志，BaseLib 会输出缺失的 key 名称。

---

## 7. 构建命令

```bash
# 编译 DLL 并复制到游戏目录
dotnet build firefly.csproj

# 导出 PCK 资源包（包含本地化文件）
dotnet build firefly.csproj -t:ExportPck

# 两者一起
dotnet build firefly.csproj
dotnet build firefly.csproj -t:ExportPck
```

**注意**: 修改本地化文件后必须重新导出 PCK！
