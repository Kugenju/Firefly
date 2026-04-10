using BaseLib.Utils;
using Firefly.Powers;
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
/// 死星过载 - 稀有攻击牌（卡池）
/// 造成 20/28 点伤害，施加5层灼热，回复6点生命值
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class SupernovaBurst : CardModel
{
    public SupernovaBurst() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new DamageVar(20m, ValueProp.Move)
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target != null)
        {
            int scorchAmount = IsUpgraded ? 7 : 5; // 升级后施加7层灼热
            int healAmount = IsUpgraded ? 8 : 6;   // 升级后回复8点生命

            // 先施加灼热
            await PowerCmd.Apply<ScorchPower>(
                cardPlay.Target,
                scorchAmount,
                cardPlay.Card.Owner?.Creature,
                this
            );

            // 造成伤害
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .Execute(choiceContext);

            // 回复生命值
            if (cardPlay.Card.Owner?.Creature != null)
            {
                await CreatureCmd.Heal(cardPlay.Card.Owner.Creature, healAmount);
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(8m);
    }
}
