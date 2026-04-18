using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Firefly.Powers;

/// <summary>
/// 每回合开始时获得能量。
/// </summary>
[Pool(typeof(FireflyPowers))]
public class WhyLifeSleepsEnergyPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override List<(string, string)> Localization => new PowerLoc(
        Title: "沉睡回响",
        Description: "每回合开始时获得{0}点能量。",
        SmartDescription: "每回合开始时获得{0}点能量。"
    );

    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side != Owner.Side)
        {
            return;
        }

        if (Owner?.Player != null && Amount > 0)
        {
            await PlayerCmd.GainEnergy(Amount, Owner.Player);
        }
    }
}

