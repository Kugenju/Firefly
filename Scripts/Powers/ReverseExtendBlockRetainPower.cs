using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Firefly.Powers;

/// <summary>
/// 目标格挡保留指定回合数。
/// </summary>
[Pool(typeof(FireflyPowers))]
public class ReverseExtendBlockRetainPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override List<(string, string)> Localization => new PowerLoc(
        Title: "格挡延申",
        Description: "保留格挡{0}回合。",
        SmartDescription: "保留格挡{0}回合。"
    );

    public override bool ShouldClearBlock(Creature creature)
    {
        return creature != Owner || Amount <= 0;
    }

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side == Owner.Side)
        {
            await PowerCmd.Decrement(this);
        }
    }
}

