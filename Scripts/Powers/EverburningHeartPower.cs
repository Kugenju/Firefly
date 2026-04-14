using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Firefly.Powers;

/// <summary>
/// 永燃之心能力 - 每回合对所有敌人施加灼热
/// </summary>
[Pool(typeof(FireflyPowers))]
public class EverburningHeartPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override List<(string, string)> Localization => new PowerLoc(
        Title: "永燃之心",
        Description: "每回合开始时，对所有敌人施加{0}层灼热。",
        SmartDescription: "每回合开始时，对所有敌人施加{0}层灼热。"
    );

    /// <summary>
    /// 回合开始时对所有敌人施加灼热
    /// </summary>
    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side != Owner.Side) return;

        // 获取所有敌人
        var enemies = combatState.Enemies
            .Where(c => c.IsAlive)
            .ToList();

        // 对每个敌人施加灼热
        foreach (var enemy in enemies)
        {
            if (enemy.IsAlive)
            {
                await PowerCmd.Apply<ScorchPower>(enemy, Amount, Owner, null, false);
            }
        }
    }
}
