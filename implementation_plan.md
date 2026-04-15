# 流萤卡牌实现计划表（到天台合影）- 类名修正版

## 概览
- **目标**: 实现 design 表中到天台合影的所有卡牌
- **总数**: 70张卡牌占位符
- **已实现**: 34张 ✅
- **待实现**: 36张 ⬜

**类名调整原则**: 英文类名与中文含义直接对应，使用驼峰命名法

---

## 第一批：普通攻击卡（Common Attack） - 5张

| 占位ID | 中文名 | 修正后类名 | 费用 | 描述 |
|--------|--------|-----------|------|------|
| ATT_COM_08 | 牺牲 | **SacrificeStrike** | 1 | 失去1点生命值。对所有敌人造成12点伤害。升级：造成18点伤害。 |
| ATT_COM_09 | 冲锋 | **ChargeStrike** | 1 | 造成8点伤害。如果本回合造成过伤害，伤害+4。升级：伤害+6。 |
| ATT_COM_10 | 迅捷斩 | **SwiftStrike** | 0 | 造成4点伤害。抽1张牌。升级：造成7点伤害，抽2张牌。 |
| ATT_COM_12 | 灼热之刃 | **ScorchingBlade** | 1 | 造成6点伤害。施加3层灼热。升级：造成9点伤害，施加4层灼热。 |
| ATT_COM_13 | 熵减宿命 | **EntropyFate** | 2 | 给予所有敌人6层格挡。对所有有格挡的敌人赋予裂解。升级：给予8层格挡。 |

**原类名**: Breakthrough → SacrificeStrike, GrappleStrike → ChargeStrike, QuickStrike → SwiftStrike, DissolutionWave → EntropyFate

**预计工时**: 2.5-4.0小时

---

## 第二批：罕见攻击卡（Uncommon Attack） - 10张

| 占位ID | 中文名 | 修正后类名 | 费用 | 描述 |
|--------|--------|-----------|------|------|
| ATT_UNC_06 | 毁灭者 | **Devastator** | 3 | 造成26点伤害。升级：造成34点伤害。 |
| ATT_UNC_07 | 升腾之焰 | **RisingFlame** | 1 | 造成5点伤害。每层灼热使伤害+2。升级：每层灼热使伤害+3。 |
| ATT_UNC_08 | 萤火群舞 | **FireflyDance** | 2 | [萤火]造成5点伤害3次。激发：造成10点伤害3次，费用1。 |
| ATT_UNC_09 | 夜色名为温柔 | **GentleNightStrike** | 1 | 造成9点伤害。如果目标有裂解，抽1张牌。升级：抽2张牌。 |
| ATT_UNC_10 | 熵之刃 | **EntropyBlade** | 2 | 造成10点伤害。移除目标所有格挡。升级：费用1。 |
| ATT_UNC_11 | 燃命 | **LifeBurn** | 0 | 失去等于当前能量值的生命值。造成等于失去生命值×7的伤害。升级：×10。 |
| ATT_UNC_12 | 散焰 | **ScatterFlame** | 2 | 对所有敌人造成8点伤害。如果有敌人在灼热状态，额外施加2层灼热。 |
| ATT_UNC_13 | 快速裂解 | **RapidDissolution** | 1 | 造成6点伤害。赋予目标裂解。如果目标已有裂解，改为造成12点伤害。 |
| ATT_UNC_14 | 萤火新星 | **FireflyNova** | 2 | [萤火]对所有敌人造成7点伤害。激发：造成14点伤害，费用1。 |
| ATT_UNC_15 | 莽撞冲锋 | **RecklessCharge** | 1 | 失去5点生命值。造成20点伤害。如果目标死于这张牌，回复7点生命。 |

**原类名变更**: FireflySwarm → FireflyDance, PrecisionStrike → GentleNightStrike, LifeBurning → LifeBurn, QuickDissolution → RapidDissolution

**预计工时**: 5.0-8.0小时

---

## 第三批：稀有攻击卡（Rare Attack） - 5张

| 占位ID | 中文名 | 修正后类名 | 费用 | 描述 |
|--------|--------|-----------|------|------|
| ATT_RAR_03 | 星火湮灭 | **StarfireAnnihilation** | 2 | [萤火]造成25点伤害。激发：造成50点伤害，施加5层灼热，回复10点生命。 |
| ATT_RAR_04 | 欧米伽爆破 | **OmegaBlast** | 3 | 消耗。对所有敌人造成20点伤害。施加5层灼热。升级：造成28点伤害。 |
| ATT_RAR_05 | 何物朝向死亡 | **TowardsDeath** | 1 | 造成15点伤害。如果目标生命值低于25%，伤害变为3倍。升级：30% threshold。 |
| ATT_RAR_06 | 静默的星河 | **SilentStars** | 2 | 对所有拥有裂解的敌人造成15点伤害。每有一个裂解敌人，获得3点格挡。 |
| ATT_RAR_07 | 飞萤之火 | **FireflyGlow** | 0 | 生命值低于10点时才能打出。对所有敌人造成等于已失去生命值的伤害。 |

**原类名变更**: OmegaBlaster → OmegaBlast, FatalTrigger → TowardsDeath, DissolutionAnnihilation → SilentStars, LastLight → FireflyGlow

**预计工时**: 2.5-4.0小时

---

## 第四批：普通技能卡（Common Skill） - 4张

| 占位ID | 中文名 | 修正后类名 | 费用 | 描述 |
|--------|--------|-----------|------|------|
| SKI_COM_05 | 晖长石的烟火 | **SpheneFireworks** | 1 | 获得1点能量。抽1张牌。升级：获得2点能量。 |
| SKI_COM_06 | 反向延申 | **ReverseExtend** | 1 | 选择一名敌人使其格挡可以保留至下一个回合。 |
| SKI_COM_07 | 赤染之茧 | **CrimsonCocoon** | 0 | 获得5点格挡。生命值每低于最大值10点，格挡+2。升级：格挡+3。 |
| SKI_COM_08 | 紧急修复 | **EmergencyRepair** | 1 | 失去2点生命值。获得8点格挡。抽1张牌。升级：获得12点格挡。 |

**原类名变更**: EnergyBoost → SpheneFireworks, ReinforcedArmor → ReverseExtend, DesperateDefense → CrimsonCocoon

**预计工时**: 2.0-3.2小时

---

## 第五批：罕见技能卡（Uncommon Skill） - 7张

| 占位ID | 中文名 | 修正后类名 | 费用 | 描述 |
|--------|--------|-----------|------|------|
| SKI_UNC_04 | 萤火之舞 | **FireflyDance** | 1 | 抽2张牌。本回合你打出的下一张攻击牌费用为0。升级：抽3张牌。 |
| SKI_UNC_05 | 赤金之梦 | **GoldenDream** | 1 | 移除一个敌人身上的所有灼热。每移除1层，回复1点生命值。升级：回复2点。 |
| SKI_UNC_06 | 从梦中醒来 | **AwakenFromDream** | 2 | 将目标敌人身上的所有格挡转移给自己。升级：转移的格挡+50%。 |
| SKI_UNC_08 | 终竟的明天 | **FinalTomorrow** | 1 | 获得6点格挡。将手牌中所有萤火牌置入弃牌堆，每张抽1张牌。升级：抽2张。 |
| SKI_UNC_10 | 燃烧意志 | **BurningWill** | 1 | 失去4点生命值。本回合获得2层力量。升级：获得3层力量。 |
| SKI_UNC_11 | 熵之盾 | **EntropyShield** | 2 | 获得等于当前失去生命值一半的格挡。升级：获得等于失去生命值的格挡。 |
| SKI_UNC_12 | 萤火燎原 | **FireIgnition** | 1 | 选择手牌中一张牌，使其获得萤火属性。抽1张牌。升级：抽2张牌。 |

**原类名变更**: ScorchAbsorb → GoldenDream, BarrierTransfer → AwakenFromDream, TacticalRetreat → FinalTomorrow, BurningSpirit → BurningWill, QuickIgnition → FireIgnition

**注意**: SKI_UNC_12 "萤火燎原" 为避免与现有 `FireflyWildfire` 重名，改为 **FireIgnition**（点燃）

**预计工时**: 3.5-5.6小时

---

## 第六批：稀有技能卡（Rare Skill） - 5张（含天台合影）

| 占位ID | 中文名 | 修正后类名 | 费用 | 描述 |
|--------|--------|-----------|------|------|
| SKI_RAR_04 | 生命因何而沉睡 | **WhyLifeSleeps** | 1 | 生命值上限减少1点。本场战斗每回合获得2点能量。升级：获得3点能量。 |
| SKI_RAR_05 | 无梦的长夜 | **DreamlessNight** | 2 | 将弃牌堆中所有萤火牌返回手牌。获得等于返回数量×2的格挡。升级：×3。 |
| SKI_RAR_06 | 时停 | **TimeHalt** | 1 | 本回合敌人不行动。你受到的所有伤害+5。消耗。升级：受到伤害+3。 |
| SKI_RAR_07 | 橡木蛋糕卷 | **OakCakeRoll** | 0 | 消耗。回复10点生命值。获得1点能量。升级：回复15点生命值。 |
| SKI_RAR_08 | 天台合影 | **RooftopPhoto** | 1 | 获得8点格挡。抽1张牌。如果手牌中有萤火牌，额外获得4点格挡并再抽1张牌。 |

**原类名变更**: LifeWhySleep → WhyLifeSleeps, FireflyCycle → DreamlessNight, TimeStop → TimeHalt, OmeletCake → OakCakeRoll

**预计工时**: 2.5-4.0小时

---

## 总计

| 批次 | 内容 | 卡牌数 | 预计工时 |
|------|------|--------|----------|
| 第一批 | 普通攻击卡 | 5 | 2.5-4.0h |
| 第二批 | 罕见攻击卡 | 10 | 5.0-8.0h |
| 第三批 | 稀有攻击卡 | 5 | 2.5-4.0h |
| 第四批 | 普通技能卡 | 4 | 2.0-3.2h |
| 第五批 | 罕见技能卡 | 7 | 3.5-5.6h |
| 第六批 | 稀有技能卡 | 5 | 2.5-4.0h |
| **总计** | | **36** | **18-28h** |

---

## 命名变更对照表

### 完全匹配的类名（无需变更）
- Devastator, RisingFlame, EntropyBlade, ScatterFlame, FireflyNova, RecklessCharge
- StarfireAnnihilation, EmergencyRepair, FireflyDance, EntropyShield, RooftopPhoto

### 需要变更的类名
| 占位ID | 原类名 | 新类名 | 变更原因 |
|--------|--------|--------|----------|
| ATT_COM_08 | Breakthrough | SacrificeStrike | 突破→牺牲 |
| ATT_COM_09 | GrappleStrike | ChargeStrike | 擒抱→冲锋 |
| ATT_COM_10 | QuickStrike | SwiftStrike | 更强调迅捷 |
| ATT_COM_13 | DissolutionWave | EntropyFate | 波→宿命 |
| ATT_UNC_08 | FireflySwarm | FireflyDance | 蜂群→群舞 |
| ATT_UNC_09 | PrecisionStrike | GentleNightStrike | 精准→夜色温柔 |
| ATT_UNC_11 | LifeBurning | LifeBurn | 更简洁 |
| ATT_UNC_13 | QuickDissolution | RapidDissolution | 更正式 |
| ATT_RAR_04 | OmegaBlaster | OmegaBlast | 爆破器→爆破 |
| ATT_RAR_05 | FatalTrigger | TowardsDeath | 触发→朝向死亡 |
| ATT_RAR_06 | DissolutionAnnihilation | SilentStars | 裂解→静默星河 |
| ATT_RAR_07 | LastLight | FireflyGlow | 最后之光→飞萤之火 |
| SKI_COM_05 | EnergyBoost | SpheneFireworks | 能量→晖长石烟火 |
| SKI_COM_06 | ReinforcedArmor | ReverseExtend | 装甲→反向延申 |
| SKI_COM_07 | DesperateDefense | CrimsonCocoon | 绝望→赤染之茧 |
| SKI_UNC_05 | ScorchAbsorb | GoldenDream | 吸收→赤金之梦 |
| SKI_UNC_06 | BarrierTransfer | AwakenFromDream | 转移→从梦中醒来 |
| SKI_UNC_08 | TacticalRetreat | FinalTomorrow | 撤退→终竟明天 |
| SKI_UNC_10 | BurningSpirit | BurningWill | 精神→意志 |
| SKI_UNC_12 | QuickIgnition | FireIgnition | 避免与FireflyWildfire重名 |
| SKI_RAR_04 | LifeWhySleep | WhyLifeSleeps | 更自然的英文 |
| SKI_RAR_05 | FireflyCycle | DreamlessNight | 循环→无梦长夜 |
| SKI_RAR_06 | TimeStop | TimeHalt | 停止→暂停 |
| SKI_RAR_07 | OmeletCake | OakCakeRoll | 煎蛋卷→橡木蛋糕卷 |

---

## 实现建议

### 优先级（按机制复杂度）
1. **第一批** - 基础攻击卡（最简单）
2. **第四批** - 基础技能卡（简单）
3. **第二批** - 攻击卡（中等）
4. **第五批** - 技能卡（中等）
5. **第六批** - 稀有技能（复杂特效）
6. **第三批** - 稀有攻击（复杂条件）

### 每批次完成后需要更新
1. 创建卡牌 C# 文件
2. 添加本地化处理
3. 添加到 `FireflyCardPool.cs`
4. 更新 design 表中的 Class 列
5. 测试游戏内效果
