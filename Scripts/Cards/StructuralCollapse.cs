using BaseLib.Utils;
using Firefly.Scripts.CardPools;
using System.Collections.Generic;
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
/// 结构崩解 (Structural Collapse) - 罕见攻击卡
///
/// "萨姆的利刃..."
///
/// 造成8点伤害。对目标敌人的格挡造成双倍伤害。
/// 升级：造成10点伤害。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class StructuralCollapse : CardModel
{
    public StructuralCollapse() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, false)
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
            GD.PrintErr("[StructuralCollapse] No target selected!");
            return;
        }

        int baseDamage = (int)DynamicVars.Damage.BaseValue;
        int targetBlock = target.Block;

        GD.Print($"[StructuralCollapse] Target has {targetBlock} block, base damage {baseDamage}");

        // 计算实际伤害
        // 如果有格挡，先造成双倍伤害破盾，然后剩余伤害攻击生命
        if (targetBlock > 0)
        {
            // 先造成双倍伤害攻击格挡
            int blockDamage = baseDamage * 2;
            GD.Print($"[StructuralCollapse] Dealing {blockDamage} damage to block (double)");

            // 对格挡造成伤害（使用Unpowered避免力量影响格挡伤害计算）
            await CreatureCmd.Damage(
                choiceContext,
                target,
                blockDamage,
                ValueProp.Unpowered,
                Owner?.Creature,
                this
            );

            GD.Print($"[StructuralCollapse] Block damage dealt. Remaining block: {target.Block}");

            // 如果格挡已破，再造成一次基础伤害攻击生命
            if (target.Block <= 0 && target.IsAlive)
            {
                GD.Print($"[StructuralCollapse] Block broken! Dealing additional {baseDamage} damage to HP");
                await CreatureCmd.Damage(
                    choiceContext,
                    target,
                    baseDamage,
                    ValueProp.Unpowered,
                    Owner?.Creature,
                    this
                );
            }
        }
        else
        {
            // 无格挡时正常造成伤害
            GD.Print($"[StructuralCollapse] No block, dealing {baseDamage} damage");
            await CreatureCmd.Damage(
                choiceContext,
                target,
                baseDamage,
                ValueProp.Unpowered,
                Owner?.Creature,
                this
            );
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);  // 8→10
    }
}
