# 文档整理报告

**整理时间**: 2026年4月9日

---

## 一、已删除的过时/冗余文档

| 文档 | 删除原因 | 内容替代 |
|------|---------|---------|
| `SETUP.md` | 环境配置步骤已整合到新 README | 新 README.md "快速开始" 章节 |
| `QUICKSTART.md` | 快速开始内容已整合到新 README | 新 README.md "快速开始" 章节 |
| `ENV_CHECK.md` | 特定于某次检查的快照，内容已过时 | 新 README.md 包含当前配置说明 |
| `CHARACTER_GUIDE.md` | 部分代码示例过时（CustomCharacterModel）| 新 README.md + TUTORIAL_04 |
| `FIX_BLACKSCREEN.md` | 特定问题的临时解决方案 | 问题已修复，内容整合到新 README |
| `KNOWLEDGE_BASE.md` | 部分内容过时，与当前实现不符 | 关键信息已整合到新 README |

---

## 二、保留的文档

| 文档 | 保留原因 | 状态 |
|------|---------|------|
| `README.md` | 主文档，已更新以当前实现为准 | ✅ 已更新 |
| `LOCALIZATION_AND_VARS.md` | 本地化和变量指南，内容完整准确 | ✅ 无需修改 |
| `TUTORIAL_04_添加新人物.md` | 详细教程，PlaceholderCharacterModel 用法正确 | ✅ 无需修改 |

---

## 三、README.md 更新要点

### 1. 以当前代码实现为准

- **角色类**: 使用 `PlaceholderCharacterModel`（而非 `CustomCharacterModel`）
- **卡牌类**: 使用 `CardModel`（而非 `CustomCardModel`）
- **遗物类**: 使用 `RelicModel`
- **资源路径**: 使用原版 Ironclad 资源作为占位符

### 2. 删除的过时信息

删除了以下内容：
- `CustomCharacterModel` 的详细配置示例
- `CustomCardModel` 的用法
- 手动注册角色的反射代码
- 手动添加本地化的翻译代码

### 3. 保留的有用信息

保留了以下内容：
- BaseLib 框架介绍
- 项目结构说明
- 快速开始步骤
- 卡牌代码完整示例
- 本地化 key 命名规则
- 调试技巧

### 4. 新增内容

- 13 张卡牌列表和说明
- 当前实现的角色定义代码
- 构建命令（DLL + PCK）
- 项目文件结构说明

---

## 四、当前代码实现概要

### 角色 (Firefly.cs)

```csharp
public sealed class Firefly : PlaceholderCharacterModel
{
    // 使用 PlaceholderCharacterModel - 缺失资源自动使用原版
    public override int StartingHp => 70;
    public override int StartingGold => 99;
    public override IEnumerable<CardModel> StartingDeck => ...;
    public override IReadOnlyList<RelicModel> StartingRelics => ...;
}
```

### 卡牌 (示例: FireflyStrike.cs)

```csharp
[Pool(typeof(FireflyCardPool))]
public class FireflyStrike : CardModel
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new DamageVar(6m, ValueProp.Move)
    };
    
    protected override async Task OnPlay(...) { ... }
    protected override void OnUpgrade() { ... }
}
```

### 本地化 (cards.json)

```json
{
    "FIREFLY_STRIKE.title": "流萤打击",
    "FIREFLY_STRIKE.description": "造成{Damage:diff()}点伤害。"
}
```

---

## 五、文档使用建议

### 新开发者

1. 首先阅读 `README.md` - 了解项目概况和快速开始
2. 开发卡牌时参考 `LOCALIZATION_AND_VARS.md` - 了解本地化规则
3. 需要自定义角色资源时阅读 `TUTORIAL_04_添加新人物.md`

### 文档维护

- `README.md` 应随代码更新而更新
- `LOCALIZATION_AND_VARS.md` 和 `TUTORIAL_04_添加新人物.md` 较为稳定
- 避免创建针对特定问题的临时文档（应直接修复问题）

---

## 六、Git 提交

建议提交信息：
```
docs: 整理文档，删除过时信息

- 删除 6 个过时/冗余文档
- 重写 README.md 以当前代码实现为准
- 保留 LOCALIZATION_AND_VARS.md 和 TUTORIAL_04
- 添加文档整理报告
```
