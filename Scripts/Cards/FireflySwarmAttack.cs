using BaseLib.Utils;
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
/// 萤火群舞 - 罕见攻击牌（萤火）
/// [萤火]造成5点伤害3次。激发：造成10点伤害3次，费用1。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class FireflySwarmAttack : FireflyCard
{
    public FireflySwarmAttack() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new DamageVar(5m, ValueProp.Move)
    };

    protected override async Task OnFireflyPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null) return;

        int multiplier = FireflyIgnitionManager.GetEffectMultiplier(this);
        int damage = (int)(DynamicVars.Damage.BaseValue * multiplier);
        int hitCount = 3;

        // 造成3次伤害
        for (int i = 0; i < hitCount; i++)
        {
            await DamageCmd.Attack(damage)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .Execute(choiceContext);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m); // 5->7
    }
}
