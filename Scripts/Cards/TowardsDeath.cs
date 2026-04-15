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
/// 何物朝向死亡 - 稀有攻击牌
/// 造成15点伤害。如果目标生命值低于25%，伤害变为3倍。升级：30% threshold。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class TowardsDeath : CardModel
{
    public TowardsDeath() : base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new DamageVar(15m, ValueProp.Move)
    };

    private const int EXECUTE_THRESHOLD = 25;
    private const int UPGRADED_THRESHOLD = 30;
    private const int MULTIPLIER = 3;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null) return;

        int baseDamage = (int)DynamicVars.Damage.BaseValue;
        int threshold = IsUpgraded ? UPGRADED_THRESHOLD : EXECUTE_THRESHOLD;

        // TODO: 获取目标生命值百分比
        int healthPercent = 50; // 默认值
        int finalDamage = baseDamage;

        // 如果生命值低于阈值，伤害3倍
        if (healthPercent < threshold)
        {
            finalDamage = baseDamage * MULTIPLIER;
        }

        await CreatureCmd.Damage(
            choiceContext,
            cardPlay.Target,
            finalDamage,
            ValueProp.Move,
            Owner?.Creature,
            this
        );
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(5m); // 15->20, 45->60
    }
}
