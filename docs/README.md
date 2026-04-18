# Firefly 文档总览

> 最后整理时间：2026-04-18  
> 目标：删除过时文档，保留有效指南，并把关键信息集中到一个入口。

## 1. 当前项目状态（代码实况）

- 角色实现：`1`（`Scripts/Characters/Firefly.cs`）
- 卡牌实现：`71`（`Scripts/Cards/*.cs`）
- 遗物实现：`6`（`Scripts/Relics/*.cs`）
- Power 实现：`17`（`Scripts/Powers/*.cs`）
- 核心机制：灼热、裂解、萤火、生存、完全燃烧已在当前代码中落地

### design 表未落地类（仍待实现）

以下 `Class名` 仍未在 `Scripts/Cards` 中找到对应类：

- `ThermalReaction`
- `FireflyResonance`
- `BlockMastery`
- `ScorchMastery`
- `DissolutionResonance`
- `LifeExchange`
- `FireflyMemory`
- `ArmorBreaker`
- `ThreeDeaths`
- `FireflyNest`
- `Inferno`
- `EntropyMaster`
- `LastStand`
- `FireflyBlessing`
- `ScorchAura`

## 2. 保留文档（有效）

- `BUILD_GUIDE.md`：构建、导出、部署到游戏目录
- `CARD_DEVELOPMENT_GUIDE.md`：卡牌开发规范与常见实现方式
- `LOCALIZATION_AND_VARS.md`：本地化与动态变量使用说明
- `FIREFLY_ST2_DESIGN_DOC.md`：角色与卡牌设计基线（设计侧）
- `firefly_card_benchmark_template.xlsx`：设计清单与实现对照（`design` 表）
- `TUTORIAL_04_添加新人物.md`：外部教程整理，主要用于角色资源与模型扩展参考

## 3. 推荐阅读顺序

1. 先看 `BUILD_GUIDE.md`，确保本地构建和部署链路可用
2. 看 `CARD_DEVELOPMENT_GUIDE.md` 与 `LOCALIZATION_AND_VARS.md`，按现有工程规范补卡
3. 对照 `FIREFLY_ST2_DESIGN_DOC.md` 与 `firefly_card_benchmark_template.xlsx` 查缺补漏

## 4. 本次整合说明

已删除以下“阶段性计划 / 历史报告 / 失效索引”文档，避免继续误导开发：

- `CARD_POOL_ANALYSIS.md`
- `CODE_AUDIT_REPORT.md`
- `DISSOLUTION_IMPLEMENTATION_PLAN.md`
- `DOCUMENT_CLEANUP_REPORT.md`
- `FIREFLY_IMPLEMENTATION_PLAN.md`
- `ITERATION_ROADMAP.md`
- `LOCALIZATION_MAPPING.md`
- `REVERSE_ENGINEERING_SETUP.md`

## 5. 文档维护约定

- 新增或重命名卡牌类后，必须同步更新 `firefly_card_benchmark_template.xlsx` 的 `design` 表
- 若机制发生行为变更，优先更新 `CARD_DEVELOPMENT_GUIDE.md`，再更新设计文档
- 不再新增“临时计划/阶段报告”到 `docs` 根目录，改为在任务讨论或提交记录中跟踪
