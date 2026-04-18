# Firefly Mod 代码审计报告

**审计日期**: 2026-04-18
**审计范围**: 全部源代码
**项目版本**: 当前 main 分支

---

## 项目概览

| 项目 | 数量 |
|------|------|
| 卡牌 | 56 张 |
| Power | 8 个 |
| 遗物 | 6 个 |
| 角色 | 1 个 |
| 附魔 | 1 个 |

**技术栈**:
- 框架: BaseLib + Godot.NET.Sdk 4.5.1
- 目标框架: .NET 9.0
- 目标游戏: Slay the Spire 2

---

## 编译问题

### 已修复

| 文件 | 问题 | 状态 |
|------|------|------|
| `Scripts/Cards/RisingFlame.cs` | 缺少 `using System.Linq;` | 已修复 |
| `Scripts/Powers/DissolutionSourcePower.cs` | `PowerModel.Amount` 只读属性赋值 | 已修复 |
| `Scripts/Powers/FireflyIgnoreBlockPower.cs` | `PowerModel.Amount` 只读属性赋值 | 已修复 |

---

## 未实现功能 (TODO)

以下卡牌存在未实现或不完整的功能：

| 文件 | 卡牌名 | 缺失功能 | 严重程度 |
|------|--------|----------|----------|
| `ChargeStrike.cs` | 冲锋 | 伤害加成逻辑被注释掉，卡牌无法正常工作 | 高 |
| `CrimsonCocoon.cs` | 赤染之茧 | 基于损失生命值的格挡加成未计算 | 中 |
| `EntropyShieldSkill.cs` | 熵之盾 | 损失生命值计算硬编码为10 | 中 |
| `FireflyDanceSkill.cs` | 萤火之舞 | "下一张攻击牌费用为0" 未实现 | 中 |
| `FireflyGlow.cs` | 飞萤之火 | HP阈值条件未实现，损失生命硬编码为30 | 中 |
| `FireIgnition.cs` | 萤火燎原 | 手牌添加萤火关键词未实现 | 低 |
| `LifeBurn.cs` | 燃命 | 当前能量值硬编码为2 | 中 |
| `ReverseExtend.cs` | 反向延申 | 格挡保留完全未实现，卡牌无效果 | 高 |
| `TimeHalt.cs` | 时停 | 两个效果都未实现，卡牌无效果 | 高 |
| `TowardsDeath.cs` | 何物朝向死亡 | 目标HP百分比硬编码为50 | 中 |
| `WhyLifeSleeps.cs` | 生命因何而沉睡 | 两个效果都未实现，卡牌无效果 | 高 |

---

## 代码架构问题

### MotherWrath.cs - 母虫之怒

**问题描述**: Power 逻辑嵌入在 Card 类中

- `AfterCardPlayed` 和 `AfterSideTurnStart` 方法应该在独立的 Power 类中实现
- `_lastBlockValues` 使用 `[NonSerialized]` 可能导致战斗序列化问题
- 灼热层数计算可能有误（使用 `blockReduced` 而非固定值1）

**建议**: 将逻辑分离到独立的 `MotherWrathPower` 类中

---

## API 使用不一致

| 问题 | 涉及文件 | 建议 |
|------|----------|------|
| 使用 `CreatureCmd.Damage` 而非 `DamageCmd.Attack` | LifeBurn, RapidDissolution, TowardsDeath | 统一使用 `DamageCmd.Attack` 进行攻击伤害 |
| Owner 获取方式不一致 | SupernovaBurst 等 | 统一使用 `Owner?.Creature` |

---

## 空遗物占位符

以下遗物仅定义稀有度，无实际功能：

| 文件 | 遗物名 | 稀有度 |
|------|--------|--------|
| `CombustionEngine.cs` | 燃烧引擎 | Uncommon |
| `FireflyWings.cs` | 萤火之翼 | Common |
| `GransoursCore.cs` | 格兰索核心 | Rare |
| `IronCavalryInsignia.cs` | 铁骑徽章 | Uncommon |

**建议**: 实现遗物功能或标记为 WIP (Work In Progress)

---

## 代码质量问题

### 1. 调试代码残留

- `FireflyIgnoreBlockPower.cs:51` - `GD.Print` 调试语句未移除

### 2. 方法命名问题

- `FireflyIgnoreBlockPower.ShouldIgnoreBlock()` - 方法有副作用（减少 Amount），但命名未体现
- 建议: 重命名为 `ShouldIgnoreBlockAndConsume()` 或类似名称

### 3. 冗余代码

| 文件 | 行号 | 问题 |
|------|------|------|
| `EverburningHeartPower.cs` | 43 | `enemy.IsAlive` 检查冗余（LINQ已过滤） |
| `OverloadCorePower.cs` | 43 | 冗余 null 检查 |

### 4. 重复 using 语句

- `DreamlessNight.cs:8` - `MegaCrit.Sts2.Core.Entities.Cards` 重复导入

### 5. Pool 属性不一致

- `FireflyIgnoreBlockPower` 缺少 `[Pool(typeof(FireflyPowers))]` 属性

---

## 编译警告

当前存在 9 个编译警告：

| 类型 | 数量 | 说明 |
|------|------|------|
| CS0105 | 1 | 重复 using 指令 |
| CS8765 | 4 | Null 性与重写成员不匹配 |
| CS8602 | 4 | 可能的空引用解引用 |

这些警告不影响编译和运行，建议在后续优化中处理。

---

## 良好实践

1. **基类设计**: `FireflyCard` 抽象类正确封装了萤火牌的激发机制
2. **命名规范**: 类名、方法名遵循 C# 惯例
3. **空值检查**: 大部分代码有适当的 null propagation (`?.`)
4. **常量使用**: 关键数值使用常量定义（如 `MAX_DAMAGE_BLOCK = 15`）
5. **异步模式**: 正确使用 `async/await` 处理游戏动作
6. **本地化支持**: 使用 `PowerLoc` 和 `DynamicVar` 支持本地化

---

## 修复优先级建议

### P0 - 必须修复（阻塞游戏功能）

1. `ChargeStrike.cs` - 取消注释伤害加成逻辑
2. `ReverseExtend.cs` - 实现格挡保留功能
3. `TimeHalt.cs` - 实现核心功能
4. `WhyLifeSleeps.cs` - 实现核心功能

### P1 - 应该修复（影响游戏体验）

1. 实现所有 TODO 标记的功能
2. 将 `MotherWrath` 的 Power 逻辑分离
3. 统一 API 使用方式
4. 实现遗物功能

### P2 - 可以改进（代码质量）

1. 移除调试代码
2. 修复重复 using 语句
3. 修复 Null 性警告
4. 统一 Pool 属性使用

---

## 统计摘要

| 类别 | 数量 |
|------|------|
| 编译错误（已修复） | 3 |
| 未实现功能 | 11 |
| 架构问题 | 1 |
| API 不一致 | 4 |
| 空占位符遗物 | 4 |
| 代码质量问题 | 6 |

---

## 结论

项目框架完整，核心机制（灼热、裂解、激发）实现良好。主要问题集中在：

1. 部分卡牌功能未完成
2. 少量遗物为占位符
3. 代码一致性可改进

建议优先完成 P0 级别的功能实现，确保所有卡牌可正常使用。
