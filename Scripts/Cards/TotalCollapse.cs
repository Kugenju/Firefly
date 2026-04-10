using BaseLib.Utils;
using Firefly.Scripts.CardPools;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Firefly.Powers;

namespace Firefly.Scripts.Cards;

/// <summary>
/// 完全崩毁 (Total Collapse) - 稀有攻击卡
/// 
/// "这就是...格拉默铁骑的力量！"
/// 
/// 造成20点伤害。对所有拥有「裂解」的敌人，无视格挡造成等同于其裂解源层数的伤害。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class TotalCollapse : CardModel
{
    public TotalCollapse() : base(3, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new DamageVar(20m, ValueProp.Move)
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var target = cardPlay.Target;
        var combatState = Owner?.Creature?.CombatState;

        if (target == null || combatState == null)
        {
            GD.PrintErr("[TotalCollapse] Invalid target or combat state!");
            return;
        }

        int baseDamage = (int)DynamicVars.Damage.BaseValue;

        // 1. 对主要目标造成基础伤害
        GD.Print($"[TotalCollapse] Dealing {baseDamage} base damage to {target.Name}...");
        await CreatureCmd.Damage(
            choiceContext,
            target,
            baseDamage,
            ValueProp.Move,
            Owner?.Creature,
            this
        );

        // 2. 对所有有裂解的敌人造成额外伤害
        var enemiesWithDissolution = combatState.Creatures
            .Where(c => c.IsMonster && c.IsAlive && c.Powers.Any(p => p is DissolutionPower))
            .ToList();

        GD.Print($"[TotalCollapse] Found {enemiesWithDissolution.Count} enemies with Dissolution");

        foreach (var enemy in enemiesWithDissolution)
        {
            // 获取裂解源层数
            var dissolutionSource = enemy.Powers
                .OfType<DissolutionSourcePower>()
                .FirstOrDefault();
            
            int dissolutionDamage = dissolutionSource?.Amount ?? 0;

            if (dissolutionDamage > 0)
            {
                GD.Print($"[TotalCollapse] Extra {dissolutionDamage} damage to {enemy.Name} (bypass block)");
                
                await CreatureCmd.Damage(
                    choiceContext,
                    enemy,
                    dissolutionDamage,
                    ValueProp.Unblockable | ValueProp.Unpowered,  // 无视格挡
                    Owner?.Creature,
                    this
                );
            }
        }

        GD.Print("[TotalCollapse] Complete!");
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(8m);  // 20→28
    }
}
