using BaseLib.Utils;
using Firefly.Powers;
using Firefly.Scripts.CardPools;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Firefly.Scripts.Cards;

/// <summary>
/// 夜色名为温柔 - 罕见攻击牌
/// 造成9点伤害。如果目标有裂解，抽1张牌。升级：抽2张牌。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class GentleNightStrike : CardModel
{
    public GentleNightStrike() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new DamageVar(9m, ValueProp.Move)
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null) return;

        // 造成伤害
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);

        // 检查目标是否有裂解
        bool hasDissolution = cardPlay.Target.Powers.Any(p => p is DissolutionPower);
        if (hasDissolution)
        {
            int drawCount = IsUpgraded ? 2 : 1;
            // 抽牌
            await CardPileCmd.Draw(choiceContext, drawCount, Owner, true);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m); // 9->12
    }
}
