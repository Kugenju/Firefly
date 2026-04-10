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
using Firefly.Powers;

namespace Firefly.Scripts.Cards;

/// <summary>
/// 熵减装甲 (Entropy Armor) - 普通技能卡
/// 
/// "为了格拉默..."
/// 
/// 获得5点格挡。选择一名敌人，使其获得6点格挡并赋予「裂解」。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class EntropyArmor : CardModel
{
    public EntropyArmor() : base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new BlockVar(5m, ValueProp.Move),              // 自身获得格挡
        new BlockVar("Enemy", 6m, ValueProp.Move)      // 敌人获得格挡（使用BlockVar以便升级）
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var target = cardPlay.Target;

        if (target == null)
        {
            GD.PrintErr("[EntropyArmor] No target selected!");
            return;
        }

        if (Owner?.Creature == null)
        {
            GD.PrintErr("[EntropyArmor] Owner is null!");
            return;
        }

        var playerCreature = Owner.Creature;
        int enemyBlock = (int)DynamicVars["Enemy"].BaseValue;

        // 1. 自身获得格挡
        GD.Print($"[EntropyArmor] Gaining {DynamicVars.Block.BaseValue} block...");
        await CreatureCmd.GainBlock(
            playerCreature,
            DynamicVars.Block,
            cardPlay,
            false
        );

        // 2. 先应用裂解源Power（在获得格挡之前）
        await PowerCmd.Apply<DissolutionSourcePower>(
            target,
            enemyBlock,
            playerCreature,
            this
        );

        // 3. 敌人获得格挡
        GD.Print($"[EntropyArmor] Enemy gaining {enemyBlock} block...");
        await CreatureCmd.GainBlock(
            target,
            enemyBlock,
            ValueProp.Move,
            cardPlay,
            false
        );

        // 4. 给敌人应用裂解标记
        await PowerCmd.Apply<DissolutionPower>(
            target,
            1,
            playerCreature,
            this
        );

        GD.Print($"[EntropyArmor] Complete! {target.Name} has Dissolution ({enemyBlock}).");
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(2m);      // 自身格挡 5→7
        DynamicVars["Enemy"].UpgradeValueBy(2m);   // 敌人格挡 6→8
    }
}
