using BaseLib.Utils;
using Firefly.Powers;
using Firefly.Scripts.CardPools;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Firefly.Scripts.Cards;

/// <summary>
/// 等离子囚笼 - 稀有技能牌
///
/// 对所有敌人造成10点伤害。将敌人身上的灼热层数翻倍。
/// 升级：造成14点伤害。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class PlasmaCage : CardModel
{
    public PlasmaCage() : base(2, CardType.Skill, CardRarity.Rare, TargetType.AllEnemies, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new DamageVar(10m, ValueProp.Move)
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var combatState = Owner?.Creature?.CombatState;
        if (combatState == null)
        {
            GD.PrintErr("[PlasmaCage] Invalid combat state!");
            return;
        }

        var enemies = combatState.HittableEnemies.ToList();
        int damage = (int)DynamicVars.Damage.BaseValue;

        // 1. 对所有敌人造成伤害
        foreach (var enemy in enemies)
        {
            if (enemy.IsAlive)
            {
                await DamageCmd.Attack(damage)
                    .FromCard(this)
                    .Targeting(enemy)
                    .Execute(choiceContext);
            }
        }

        // 2. 将敌人身上的灼热层数翻倍
        foreach (var enemy in enemies)
        {
            if (enemy.IsAlive)
            {
                var scorchPower = enemy.Powers.OfType<ScorchPower>().FirstOrDefault();
                if (scorchPower != null && scorchPower.Amount > 0)
                {
                    int currentStacks = scorchPower.Amount;
                    int additionalStacks = currentStacks;  // 翻倍 = 当前层数

                    GD.Print($"[PlasmaCage] Doubling Scorch on {enemy.Name}: {currentStacks} -> {currentStacks * 2}");

                    await PowerCmd.Apply<ScorchPower>(
                        enemy,
                        additionalStacks,
                        Owner?.Creature,
                        this
                    );
                }
            }
        }

        GD.Print("[PlasmaCage] Complete!");
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4m);  // 10->14
    }
}
