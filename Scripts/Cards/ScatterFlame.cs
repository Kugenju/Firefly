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
/// 散焰 - 罕见攻击牌
/// 对所有敌人造成8点伤害。如果有敌人在灼热状态，额外施加2层灼热。升级：造成11点伤害。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class ScatterFlame : CardModel
{
    public ScatterFlame() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new DamageVar(8m, ValueProp.Move)
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var combatState = Owner?.Creature?.CombatState;
        if (combatState == null) return;

        // 检查是否有敌人在灼热状态
        bool hasScorchedEnemy = combatState.HittableEnemies.Any(e => 
            e.IsAlive && e.Powers.Any(p => p is ScorchPower));

        int extraScorch = IsUpgraded ? 3 : 2;

        // 对所有敌人造成伤害和灼热
        foreach (var enemy in combatState.HittableEnemies)
        {
            if (enemy.IsAlive)
            {
                // 如果有敌人在灼热状态，额外施加灼热
                if (hasScorchedEnemy)
                {
                    await PowerCmd.Apply<ScorchPower>(
                        enemy,
                        extraScorch,
                        Owner?.Creature,
                        this
                    );
                }

                // 造成伤害
                await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                    .FromCard(this)
                    .Targeting(enemy)
                    .Execute(choiceContext);
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m); // 8->11
    }
}
