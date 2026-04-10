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
/// 熵流逆转 (Entropy Reversal) - 稀有技能卡
/// 
/// "从物品，变成人..."
/// 
/// 移除一名敌人身上所有格挡。每移除1点格挡，对其造成3点伤害。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class EntropyReversal : CardModel
{
    public EntropyReversal() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new DamageVar(3m, ValueProp.Unpowered)  // 每点格挡造成的伤害
    };
    
    private int GetDamagePerBlock() => IsUpgraded ? 5 : 3;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var target = cardPlay.Target;

        if (target == null)
        {
            GD.PrintErr("[EntropyReversal] No target selected!");
            return;
        }

        int blockRemoved = target.Block;
        int damagePerBlock = GetDamagePerBlock();

        if (blockRemoved > 0)
        {
            GD.Print($"[EntropyReversal] Removing {blockRemoved} block from {target.Name}...");

            // 1. 移除所有格挡
            await CreatureCmd.LoseBlock(target, blockRemoved);

            // 2. 计算伤害
            int totalDamage = blockRemoved * damagePerBlock;
            GD.Print($"[EntropyReversal] Dealing {totalDamage} damage ({blockRemoved} × {damagePerBlock})...");

            // 3. 造成伤害
            await CreatureCmd.Damage(
                choiceContext,
                target,
                totalDamage,
                ValueProp.Unpowered | ValueProp.Unblockable,  // 无视格挡（已移除）且不受力量影响
                Owner?.Creature,
                this
            );

            GD.Print($"[EntropyReversal] Dealt {totalDamage} damage to {target.Name}");
        }
        else
        {
            GD.Print("[EntropyReversal] Target has no block to remove.");
        }
    }

    protected override void OnUpgrade()
    {
        // 升级效果在 GetDamagePerBlock() 中处理
    }
}
