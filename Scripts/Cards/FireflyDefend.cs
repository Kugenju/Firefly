using BaseLib.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Firefly.Scripts.CardPools;

namespace Firefly.Scripts.Cards;

/// <summary>
/// 流萤的防御卡
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class FireflyDefend : CardModel
{
    public FireflyDefend() 
        : base(1, CardType.Skill, CardRarity.Basic, TargetType.Self, true)
    {
    }

    public override bool GainsBlock => true;

    protected override HashSet<CardTag> CanonicalTags => 
        new HashSet<CardTag> { CardTag.Defend };

    protected override IEnumerable<DynamicVar> CanonicalVars => 
        new List<DynamicVar> { new BlockVar(5m, ValueProp.Move) };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner?.Creature != null)
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay, false);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3m);
    }
}
