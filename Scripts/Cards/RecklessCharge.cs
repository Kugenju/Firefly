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
/// 莽撞冲锋 - 罕见攻击牌
/// 失去5点生命值。造成20点伤害。如果目标死于这张牌，回复7点生命。升级：造成28点伤害。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class RecklessCharge : CardModel
{
    public RecklessCharge() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new DamageVar(20m, ValueProp.Move)
    };

    private const int HEALTH_COST = 5;
    private const int HEAL_AMOUNT = 7;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null || Owner?.Creature == null) return;

        // 记录目标是否存活
        bool wasAlive = cardPlay.Target.IsAlive;

        // 先失去生命值
        await CreatureCmd.Damage(
            choiceContext,
            Owner.Creature,
            HEALTH_COST,
            ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move,
            null,
            this
        );

        // 造成伤害
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);

        // 如果目标死于这张牌，回复生命
        if (wasAlive && !cardPlay.Target.IsAlive)
        {
            await CreatureCmd.Heal(Owner.Creature, HEAL_AMOUNT);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(8m); // 20->28
    }
}
