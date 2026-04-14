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
/// 烈焰吞噬 - 罕见攻击牌
/// 费用0。失去5点生命值。造成12点伤害。升级：造成16点伤害。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class FlameDevour : CardModel
{
    public FlameDevour() : base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new DamageVar(12m, ValueProp.Move)
    };

    // 失去的生命值
    private const int HEALTH_COST = 5;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 先失去生命值
        if (Owner?.Creature != null)
        {
            await CreatureCmd.Damage(choiceContext, Owner.Creature, HEALTH_COST, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, null, this);
        }

        // 然后造成伤害
        if (cardPlay.Target != null)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .Execute(choiceContext);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4m);  // 12->16
    }
}
