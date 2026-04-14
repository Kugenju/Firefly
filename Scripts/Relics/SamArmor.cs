using BaseLib.Utils;
using Firefly.Powers;
using Firefly.Scripts.RelicPools;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Firefly.Scripts.Relics;

/// <summary>
/// 萨姆装甲：每打出4张牌，获得1层萤火护盾。
/// </summary>
[Pool(typeof(FireflyRelicPool))]
public class SamArmor : RelicModel
{
    private const int CardsThreshold = 4;

    private int _cardsPlayedThisCombat;

    public override RelicRarity Rarity => RelicRarity.Starter;

    public override bool ShowCounter => CombatManager.Instance.IsInProgress;

    public override int DisplayAmount
    {
        get
        {
            int remainder = _cardsPlayedThisCombat % CardsThreshold;
            return CardsThreshold - remainder;
        }
    }

    public override Task BeforeCombatStart()
    {
        _cardsPlayedThisCombat = 0;
        InvokeDisplayAmountChanged();
        return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner)
        {
            return;
        }

        if (!CombatManager.Instance.IsInProgress)
        {
            return;
        }

        _cardsPlayedThisCombat++;

        if (_cardsPlayedThisCombat % CardsThreshold == 0 && Owner?.Creature != null)
        {
            Flash();
            await PowerCmd.Apply<FireflyFlickerShieldPower>(Owner.Creature, 1, Owner.Creature, null);
        }

        InvokeDisplayAmountChanged();
    }
}
