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
/// 快速裂解 - 罕见攻击牌
/// 造成6点伤害。赋予目标裂解。如果目标已有裂解，改为造成12点伤害。升级：造成9/18点伤害。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class RapidDissolution : CardModel
{
    public RapidDissolution() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new DamageVar(6m, ValueProp.Move)
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null) return;

        // 检查目标是否已有裂解
        bool hasDissolution = cardPlay.Target.Powers.Any(p => p is DissolutionPower);

        int baseDamage = (int)DynamicVars.Damage.BaseValue;
        int finalDamage = hasDissolution ? baseDamage * 2 : baseDamage;

        // 如果没有裂解，赋予裂解
        if (!hasDissolution)
        {
            await PowerCmd.Apply<DissolutionPower>(
                cardPlay.Target,
                1,
                Owner?.Creature,
                this
            );
        }

        // 造成伤害
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
        DynamicVars.Damage.UpgradeValueBy(3m); // 6->9, 12->18
    }
}
