# -*- coding: utf-8 -*-
import sys
sys.stdout.reconfigure(encoding='utf-8')

# Excel设计 vs 代码实现对比
comparison = [
    {
        "id": "ATT_BAS_01",
        "cn_name": "打击",
        "class_name": "FireflyStrike",
        "excel_effect": "造成6点伤害。升级：造成9点伤害。",
        "code_effect": "造成6点伤害。升级：造成9点伤害。",
        "status": "匹配",
        "action": "无需修改"
    },
    {
        "id": "ATT_COM_01",
        "cn_name": "灰烬之刃",
        "class_name": "EmberBlade",
        "excel_effect": "造成9点伤害。给予一层虚弱。升级：造成12点伤害，给予2层虚弱。",
        "code_effect": "失去3点生命值。造成9点伤害。升级：造成12点伤害。",
        "status": "需修改",
        "action": "重做效果：移除自伤，改为给予虚弱"
    },
    {
        "id": "ATT_COM_02",
        "cn_name": "火萤斩击",
        "class_name": "FlameLash",
        "excel_effect": "造成7点伤害。施加2层灼热。升级：施加3层灼热。",
        "code_effect": "造成7点伤害两次。施加2层灼热。",
        "status": "需修改",
        "action": "改为单次伤害，移除两次攻击"
    },
    {
        "id": "ATT_COM_03",
        "cn_name": "闪燃打击",
        "class_name": "FlashIgniteStrike",
        "excel_effect": "[萤火]造成8点伤害。激发：造成16点伤害，耗能0。",
        "code_effect": "[萤火]造成8点伤害。激发：造成16点伤害，耗能0。",
        "status": "匹配",
        "action": "无需修改"
    },
    {
        "id": "ATT_COM_04",
        "cn_name": "结构崩解",
        "class_name": "StructuralCollapse",
        "excel_effect": "造成8点伤害。对格挡双倍伤害。升级：造成10点伤害。稀有度:Uncommon",
        "code_effect": "造成12点伤害。对格挡双倍伤害。升级：造成18点伤害。稀有度:Common",
        "status": "需修改",
        "action": "调整伤害值(8/10)，修改稀有度为Uncommon"
    },
    {
        "id": "ATT_COM_05",
        "cn_name": "裂解打击",
        "class_name": "DissolutionStrike",
        "excel_effect": "造成8点伤害。若目标有裂解且无格挡，伤害翻倍。",
        "code_effect": "造成8点伤害。若目标有裂解且无格挡，伤害翻倍。",
        "status": "匹配",
        "action": "无需修改"
    },
    {
        "id": "ATT_UNC_01",
        "cn_name": "女王低语",
        "class_name": "ChrysalidPyronexus",
        "excel_effect": "给予2层虚弱，给予2层易伤。消耗。升级：去除消耗。稀有度:Common",
        "code_effect": "造成8点伤害。升级：造成11点伤害。稀有度:Uncommon",
        "status": "需重做",
        "action": "完全重做效果和稀有度"
    },
    {
        "id": "ATT_UNC_02",
        "cn_name": "撕裂虫群",
        "class_name": "MeteoricIncineration",
        "excel_effect": "对所有敌人造成8点伤害，施加3层灼热。",
        "code_effect": "对所有敌人造成8点伤害，施加3层灼热。",
        "status": "匹配",
        "action": "无需修改"
    },
    {
        "id": "ATT_UNC_03",
        "cn_name": "点燃冲刺",
        "class_name": "IgnitionDash",
        "excel_effect": "造成8点伤害，施加4层灼热。",
        "code_effect": "造成8点伤害，施加4层灼热。",
        "status": "匹配",
        "action": "无需修改"
    },
    {
        "id": "ATT_UNC_04",
        "cn_name": "烈焰吞噬",
        "class_name": "FlameDevour",
        "excel_effect": "费用0。失去5点生命值。造成12点伤害。",
        "code_effect": "费用2。失去5点生命值。造成12点伤害。",
        "status": "需修改",
        "action": "修改费用从2改为0"
    },
    {
        "id": "ATT_UNC_05",
        "cn_name": "荧火突刺",
        "class_name": "FluorescentThrust",
        "excel_effect": "[萤火]造成15点伤害。本回合获得2层力量。",
        "code_effect": "[萤火]造成15点伤害。获得2层临时力量。",
        "status": "需确认",
        "action": "确认力量是否为本回合临时"
    },
    {
        "id": "ATT_RAR_01",
        "cn_name": "死星过载",
        "class_name": "SupernovaBurst",
        "excel_effect": "造成20点伤害，施加5层灼热，回复6点生命值。",
        "code_effect": "造成20点伤害，施加5层灼热，回复6点生命值。",
        "status": "匹配",
        "action": "无需修改"
    },
    {
        "id": "ATT_RAR_02",
        "cn_name": "完全崩毁",
        "class_name": "TotalCollapse",
        "excel_effect": "对所有有裂解的敌人，无视格挡造成等同于裂解源层数的伤害。升级：2费。",
        "code_effect": "造成20点伤害。对所有有裂解的敌人造成裂解源层数伤害。",
        "status": "需修改",
        "action": "移除基础伤害，升级改为减费"
    },
    {
        "id": "SKI_BAS_01",
        "cn_name": "防御",
        "class_name": "FireflyDefend",
        "excel_effect": "获得5点格挡。升级：获得8点格挡。",
        "code_effect": "获得5点格挡。升级：获得8点格挡。",
        "status": "匹配",
        "action": "无需修改"
    },
    {
        "id": "SKI_BAS_02",
        "cn_name": "熵增转移",
        "class_name": "EntropyTransfer",
        "excel_effect": "将自身所有格挡转移给目标敌人，并赋予裂解。",
        "code_effect": "将自身所有格挡转移给目标敌人，并赋予裂解。",
        "status": "匹配",
        "action": "无需修改"
    },
    {
        "id": "SKI_COM_01",
        "cn_name": "该醒了…",
        "class_name": "ArmoredDefense",
        "excel_effect": "[萤火]获得8点格挡。激发：获得16点格挡，耗能0。",
        "code_effect": "[萤火]获得8点格挡。激发：获得16点格挡，耗能0。",
        "status": "匹配",
        "action": "无需修改"
    },
    {
        "id": "SKI_COM_02",
        "cn_name": "熵减装甲",
        "class_name": "EntropyArmor",
        "excel_effect": "获得5点格挡。使敌人获得6点格挡并赋予裂解。",
        "code_effect": "获得5点格挡。使敌人获得6点格挡并赋予裂解。",
        "status": "匹配",
        "action": "无需修改"
    },
    {
        "id": "SKI_COM_03",
        "cn_name": "血茧",
        "class_name": "CombustionShield",
        "excel_effect": "失去2点生命值。获得15点格挡。",
        "code_effect": "失去4点生命值。获得15点格挡。",
        "status": "需修改",
        "action": "修改生命消耗从4改为2"
    },
    {
        "id": "SKI_COM_04",
        "cn_name": "热能护盾",
        "class_name": "HeatShield",
        "excel_effect": "获得8点格挡。获得2层灼热。",
        "code_effect": "获得8点格挡。获得2层灼热。",
        "status": "匹配",
        "action": "无需修改"
    },
    {
        "id": "SKI_UNC_01",
        "cn_name": "下一片战场。",
        "class_name": "FireflyWildfire",
        "excel_effect": "[萤火]抽2张牌，每张萤火牌额外抽1张。",
        "code_effect": "[萤火]抽2张牌，每张萤火牌额外抽1张。",
        "status": "匹配",
        "action": "无需修改"
    },
    {
        "id": "SKI_UNC_02",
        "cn_name": "熵流逆转",
        "class_name": "EntropyReversal",
        "excel_effect": "移除格挡，每点造成3点伤害。稀有度:Race",
        "code_effect": "移除格挡，每点造成3点伤害。稀有度:Uncommon",
        "status": "需确认",
        "action": "确认稀有度(Race可能是Rare)"
    },
    {
        "id": "SKI_UNC_03",
        "cn_name": "格拉默！格拉默！",
        "class_name": "FireflyShield",
        "excel_effect": "[萤火]获得15点格挡。下回合获得2点能量。费用1。激发：费用0。",
        "code_effect": "[萤火]获得15点格挡。下回合获得5点能量。费用2。",
        "status": "需修改",
        "action": "修改费用(1)，能量值(2/4)"
    },
    {
        "id": "SKI_RAR_01",
        "cn_name": "点燃大海",
        "class_name": "IgniteTheSea",
        "excel_effect": "激发手牌中所有萤火牌。稀有度:Uncommon",
        "code_effect": "激发手牌中所有萤火牌。稀有度:Rare",
        "status": "需修改",
        "action": "修改稀有度从Rare改为Uncommon"
    },
    {
        "id": "SKI_RAR_02",
        "cn_name": "海一直燃",
        "class_name": "FlamesSpread",
        "excel_effect": "打出手牌中所有的萤火牌。",
        "code_effect": "打出手牌中所有的萤火牌。",
        "status": "匹配",
        "action": "无需修改"
    },
    {
        "id": "SKI_RAR_03",
        "cn_name": "等离子囚笼",
        "class_name": "PlasmaCage",
        "excel_effect": "对所有敌人造成10点伤害。灼热层数翻倍。",
        "code_effect": "待检查",
        "status": "待检查",
        "action": "检查并修改"
    },
]

# 统计
match_count = sum(1 for c in comparison if c["status"] == "匹配")
need_modify = sum(1 for c in comparison if "修改" in c["status"])
need_confirm = sum(1 for c in comparison if "确认" in c["status"] or "检查" in c["status"])

print("=" * 80)
print("卡牌设计对比报告 - Excel设计 vs 代码实现")
print("=" * 80)

print(f"\n【统计摘要】")
print(f"  匹配: {match_count}张")
print(f"  需修改: {need_modify}张")
print(f"  需确认: {need_confirm}张")
print(f"  总计: {len(comparison)}张")

print("\n" + "=" * 80)
print("【详细对比】")
print("=" * 80)

for c in comparison:
    print(f"\n{c['id']} - {c['cn_name']} ({c['class_name']})")
    print(f"  Excel: {c['excel_effect']}")
    print(f"  Code:  {c['code_effect']}")
    print(f"  状态: {c['status']}")
    print(f"  操作: {c['action']}")

print("\n" + "=" * 80)
print("【修改清单】")
print("=" * 80)

print("\n[重做] 需要完全重做的卡牌:")
for c in comparison:
    if "重做" in c["action"]:
        print(f"  - {c['cn_name']} ({c['class_name']}): {c['action']}")

print("\n[修改] 需要修改效果的卡牌:")
for c in comparison:
    if "修改" in c["action"] and "重做" not in c["action"]:
        print(f"  - {c['cn_name']} ({c['class_name']}): {c['action']}")

print("\n[确认] 需要确认的卡牌:")
for c in comparison:
    if "确认" in c["action"] or "检查" in c["action"]:
        print(f"  - {c['cn_name']} ({c['class_name']}): {c['action']}")
