using BaseLib.Utils;
using Firefly.Scripts.CardPools;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Firefly.Scripts.Cards;

/// <summary>
/// 燃烧护盾 - 罕见技能牌
/// 获得 15/20 点格挡
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class CombustionShield : CardModel
{
    public CombustionShield() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new BlockVar(15m, ValueProp.Move)
    };

    // 失去的生命值
    private const int HEALTH_COST = 4;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 先失去生命值
        if (Owner?.Creature != null)
        {
            await CreatureCmd.Damage(choiceContext, Owner.Creature, HEALTH_COST, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, null, this);
        }

        // 然后获得格挡
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay, false);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(5m);
    }
}
