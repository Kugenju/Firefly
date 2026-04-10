using BaseLib.Utils;
using Firefly.Scripts.CardPools;
using System;
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
using Firefly.Powers;

namespace Firefly.Scripts.Cards;

/// <summary>
/// 熵增转移 (Entropy Transfer) - 初始卡组技能卡
/// 
/// "失熵的另一种形式..."
/// 
/// 0费：将自身所有格挡转移给目标敌人，并赋予「裂解」。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class EntropyTransfer : CardModel
{
    public EntropyTransfer() : base(0, CardType.Skill, CardRarity.Basic, TargetType.AnyEnemy, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => Array.Empty<DynamicVar>();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var target = cardPlay.Target;

        if (target == null)
        {
            GD.PrintErr("[EntropyTransfer] No target selected!");
            return;
        }

        if (Owner?.Creature == null)
        {
            GD.PrintErr("[EntropyTransfer] Owner is null!");
            return;
        }

        var playerCreature = Owner.Creature;

        // 1. 获取要转移的格挡数（玩家当前所有格挡）
        int blockToTransfer = playerCreature.Block;
        
        if (blockToTransfer > 0)
        {
            GD.Print($"[EntropyTransfer] Transferring {blockToTransfer} block to {target.Name}...");

            // 2. 玩家失去格挡
            await CreatureCmd.LoseBlock(playerCreature, blockToTransfer);

            // 3. 先应用裂解源Power（在获得格挡之前，这样AfterBlockGained可以正确累加）
            // 使用PowerCmd.Apply的amount参数直接设置计数
            await PowerCmd.Apply<DissolutionSourcePower>(
                target,
                blockToTransfer,  // 直接设置为转移的格挡数
                playerCreature,
                this
            );

            // 4. 敌人获得格挡
            await CreatureCmd.GainBlock(
                target,
                blockToTransfer,
                ValueProp.Move,
                cardPlay,
                false
            );

            // 5. 给敌人应用裂解标记Power
            await PowerCmd.Apply<DissolutionPower>(
                target,
                1,
                playerCreature,
                this
            );

            GD.Print($"[EntropyTransfer] Transfer complete! {target.Name} now has {target.Block} block and Dissolution ({blockToTransfer}).");
        }
        else
        {
            GD.Print("[EntropyTransfer] No block to transfer.");
            
            // 即使没有格挡转移，也给予裂解标记（用于配合其他卡牌效果）
            await PowerCmd.Apply<DissolutionPower>(
                target,
                1,
                playerCreature,
                this
            );
        }
    }

    protected override void OnUpgrade()
    {
        // 升级效果：转移时额外获得2点格挡
        // 由于改为0费不获得格挡，升级效果改为：转移的格挡数+2
        // 这个效果需要在OnPlay中实现（通过检查IsUpgraded）
    }
}
