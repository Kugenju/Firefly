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
/// 灰烬之刃 - 普通攻击牌
/// 造成9点伤害。给予1层虚弱。升级：造成12点伤害，给予2层虚弱。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class EmberBlade : CardModel
{
    public EmberBlade() : base(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new DamageVar(9m, ValueProp.Move)
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target != null)
        {
            // 造成伤害
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .Execute(choiceContext);

            // 给予虚弱
            int weakAmount = IsUpgraded ? 2 : 1;
            await PowerCmd.Apply<MegaCrit.Sts2.Core.Models.Powers.WeakPower>(
                cardPlay.Target,
                weakAmount,
                Owner?.Creature,
                this
            );
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);  // 9->12
    }
}
