using BaseLib.Utils;
using Firefly.Scripts.CardPools;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Firefly.Powers;

namespace Firefly.Scripts.Cards;

/// <summary>
/// 裂解打击 (Dissolution Strike) - 普通攻击卡
/// 
/// "向死而生..."
/// 
/// 造成8点伤害。若目标有「裂解」且无格挡，伤害翻倍。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class DissolutionStrike : CardModel
{
    public DissolutionStrike() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new DamageVar(8m, ValueProp.Move)
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var target = cardPlay.Target;

        if (target == null)
        {
            GD.PrintErr("[DissolutionStrike] No target selected!");
            return;
        }

        int baseDamage = (int)DynamicVars.Damage.BaseValue;
        int finalDamage = baseDamage;

        // 检查目标是否有裂解且无格挡
        bool hasDissolution = target.Powers.Any(p => p is DissolutionPower);
        bool hasNoBlock = target.Block <= 0;

        GD.Print($"[DissolutionStrike] Target: {target.Name}, HasDissolution: {hasDissolution}, HasNoBlock: {hasNoBlock}");

        if (hasDissolution && hasNoBlock)
        {
            finalDamage = baseDamage * 2;
            GD.Print($"[DissolutionStrike] Bonus triggered! Damage: {baseDamage} → {finalDamage}");
        }

        // 造成伤害
        await CreatureCmd.Damage(
            choiceContext,
            target,
            finalDamage,
            ValueProp.Move,
            Owner?.Creature,
            this
        );

        GD.Print($"[DissolutionStrike] Dealt {finalDamage} damage to {target.Name}");
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4m);  // 8→12
    }
}
