using BaseLib.Utils;
using Firefly.Powers;
using Firefly.Scripts.CardPools;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Firefly.Scripts.Cards;

/// <summary>
/// 星火湮灭 - 稀有攻击牌（萤火）
/// [萤火]造成25点伤害。激发：造成50点伤害，施加5层灼热，回复10点生命。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class StarfireAnnihilation : FireflyCard
{
    public StarfireAnnihilation() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new DamageVar(25m, ValueProp.Move)
    };

    protected override async Task OnFireflyPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null || Owner?.Creature == null) return;

        int multiplier = FireflyIgnitionManager.GetEffectMultiplier(this);
        int damage = (int)(DynamicVars.Damage.BaseValue * multiplier);

        // 如果是激发状态，施加灼热和回血
        if (multiplier > 1)
        {
            await PowerCmd.Apply<ScorchPower>(
                cardPlay.Target,
                5,
                Owner.Creature,
                this
            );

            await CreatureCmd.Heal(Owner.Creature, 10);
        }

        // 造成伤害
        await DamageCmd.Attack(damage)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(8m); // 25->33, 50->66
    }
}
