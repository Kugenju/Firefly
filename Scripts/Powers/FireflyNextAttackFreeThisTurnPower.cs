using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Firefly.Powers;

/// <summary>
/// 本回合下一张攻击牌费用变为0。
/// </summary>
[Pool(typeof(FireflyPowers))]
public class FireflyNextAttackFreeThisTurnPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override List<(string, string)> Localization => new PowerLoc(
        Title: "萤火之舞",
        Description: "本回合你的下一张攻击牌费用变为0。",
        SmartDescription: "本回合你的下一张攻击牌费用变为0。"
    );

    public override bool TryModifyEnergyCostInCombat(CardModel card, decimal originalCost, out decimal modifiedCost)
    {
        modifiedCost = originalCost;

        if (card.Owner?.Creature != Owner)
        {
            return false;
        }

        if (card.Type != CardType.Attack)
        {
            return false;
        }

        var pileType = card.Pile?.Type;
        if (pileType != PileType.Hand && pileType != PileType.Play)
        {
            return false;
        }

        modifiedCost = 0m;
        return true;
    }

    public override async Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner?.Creature != Owner)
        {
            return;
        }

        if (cardPlay.Card.Type != CardType.Attack)
        {
            return;
        }

        var pileType = cardPlay.Card.Pile?.Type;
        if (pileType == PileType.Hand || pileType == PileType.Play)
        {
            await PowerCmd.Decrement(this);
        }
    }

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side == Owner.Side)
        {
            await PowerCmd.Remove(this);
        }
    }
}
