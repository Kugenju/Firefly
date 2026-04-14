using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Firefly.Powers;

/// <summary>
/// 萤火护盾：抵消下一次受到的最多15点伤害，持续1回合。
/// </summary>
[Pool(typeof(FireflyPowers))]
public class FireflyFlickerShieldPower : CustomPowerModel
{
    private const int MAX_DAMAGE_BLOCK = 15;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override List<(string, string)> Localization => new PowerLoc(
        Title: "萤火护盾",
        Description: "抵消下一次受到的最多15点伤害。持续1回合。",
        SmartDescription: "抵消下一次受到的最多15点伤害。持续1回合。"
    );

    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target != Owner || amount <= 0)
        {
            return 0m;
        }

        // ModifyDamageAdditive expects a delta, not the final value.
        decimal damageToBlock = System.Math.Min(amount, MAX_DAMAGE_BLOCK);
        return -damageToBlock;
    }

    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target != Owner || result.TotalDamage <= 0)
        {
            return;
        }

        await PowerCmd.Decrement(this);
    }

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (Owner.Side != side)
        {
            await PowerCmd.Remove(this);
        }
    }
}
