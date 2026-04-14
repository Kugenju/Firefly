using BaseLib.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firefly.Scripts.CardPools;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Firefly.Powers;
using Godot;
using System.Linq;

namespace Firefly.Scripts.Cards;

/// <summary>
/// 完全崩毁 (Total Collapse) - 稀有攻击卡
///
/// "这就是...格拉默铁骑的力量！"
///
/// 对所有拥有「裂解」的敌人，无视格挡造成等同于其裂解源层数的伤害。
/// 升级：费用从3减少到2。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class TotalCollapse : CardModel
{
    public TotalCollapse() : base(3, CardType.Attack, CardRarity.Rare, TargetType.Self, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => System.Array.Empty<DynamicVar>();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var combatState = Owner?.Creature?.CombatState;

        if (combatState == null)
        {
            GD.PrintErr("[TotalCollapse] Invalid combat state!");
            return;
        }

        // 对所有有裂解的敌人造成伤害
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
                GD.Print($"[TotalCollapse] Dealing {dissolutionDamage} damage to {enemy.Name} (bypass block)");

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
        // 升级后费用从3减少到2
        EnergyCost.SetThisCombat(2);
    }
}
