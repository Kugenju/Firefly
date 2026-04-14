using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Firefly.Powers;

/// <summary>
/// 流萤能量预备 - 下回合获得能量
/// </summary>
[Pool(typeof(FireflyPowers))]
public class FireflyNextTurnEnergyPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override List<(string, string)> Localization => new PowerLoc(
        Title: "能量预备",
        Description: "下回合开始时获得{0}点能量。",
        SmartDescription: "下回合开始时获得{0}点能量。"
    );

    /// <summary>
    /// 回合开始时获得能量，然后移除自己
    /// </summary>
    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side != Owner.Side) return;

        // 获得能量
        if (Owner?.Player != null && Amount > 0)
        {
            await PlayerCmd.GainEnergy(Amount, Owner.Player);
        }

        // 移除自己
        await PowerCmd.Remove(this);
    }
}
