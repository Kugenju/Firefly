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
/// 萤火新星 - 罕见攻击牌（萤火）
/// [萤火]对所有敌人造成7点伤害。激发：造成14点伤害，费用1。升级：造成10/20点伤害。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class FireflyNova : FireflyCard
{
    public FireflyNova() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new DamageVar(7m, ValueProp.Move)
    };

    protected override async Task OnFireflyPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var combatState = Owner?.Creature?.CombatState;
        if (combatState == null) return;

        int multiplier = FireflyIgnitionManager.GetEffectMultiplier(this);
        int damage = (int)(DynamicVars.Damage.BaseValue * multiplier);

        // 对所有敌人造成伤害
        foreach (var enemy in combatState.HittableEnemies)
        {
            if (enemy.IsAlive)
            {
                await DamageCmd.Attack(damage)
                    .FromCard(this)
                    .Targeting(enemy)
                    .Execute(choiceContext);
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m); // 7->10, 14->20
    }
}
