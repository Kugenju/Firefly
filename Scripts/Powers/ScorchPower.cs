using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.ValueProps;

namespace Firefly.Powers;

/// <summary>
/// 灼热 (Scorch) - 核心机制
/// 
/// 敌人效果：当显示攻击意图时，每次攻击受到等于当前层数的伤害，然后层数减少1。
/// 玩家效果：打出攻击牌时，受到等于当前层数的伤害，然后层数减少1。
/// </summary>
public class ScorchPower : CustomPowerModel
{
    // Power 类型定义
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    /// <summary>
    /// 本地化定义
    /// </summary>
    public override List<(string, string)> Localization => new PowerLoc(
        Title: "灼热",
        Description: "当显示攻击意图或打出攻击牌时，受到等于当前层数的伤害，然后层数减少1。",
        SmartDescription: "当显示攻击意图或打出攻击牌时，受到等于当前层数的伤害，然后层数减少1。"
    );

    /// <summary>
    /// 回合开始时触发灼热伤害（敌人效果）
    /// </summary>
    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        // 只在我们所在的阵营回合开始时触发
        if (side != Owner.Side)
        {
            return;
        }

        // 玩家灼热在打出卡牌时触发，不在回合开始时触发
        if (Owner.IsPlayer)
        {
            return;
        }

        // 确保还有层数
        if (Amount <= 0)
        {
            return;
        }

        // 获取攻击意图的总次数
        int attackCount = GetAttackIntentCount();
        if (attackCount <= 0)
        {
            GD.Print($"[ScorchPower] {Owner.Name} has no attack intent, skipping.");
            return;
        }

        GD.Print($"[ScorchPower] {Owner.Name} has {attackCount} attack(s), starting scorch damage. Current Amount: {Amount}");

        // 每次攻击触发一次灼热伤害
        for (int i = 0; i < attackCount; i++)
        {
            if (Amount <= 0) break;
            await TriggerSingleScorchDamage(i + 1);
        }

        GD.Print($"[ScorchPower] Finished. Final Amount: {Amount}");
    }

    /// <summary>
    /// 卡牌打出前触发灼热伤害（玩家效果）
    /// </summary>
    public override async Task BeforeCardPlayed(CardPlay cardPlay)
    {
        // 只有玩家才会在出牌时触发
        if (!Owner.IsPlayer)
        {
            return;
        }

        // 确保还有层数
        if (Amount <= 0)
        {
            return;
        }

        var card = cardPlay.Card;
        
        // 只处理攻击牌
        if (card.Type != CardType.Attack)
        {
            return;
        }

        GD.Print($"[ScorchPower] Player playing attack card {card.GetType().Name}, triggering scorch. Current Amount: {Amount}");

        // 打出攻击牌触发一次灼热伤害
        await TriggerSingleScorchDamage(1, "playing attack card");

        GD.Print($"[ScorchPower] Finished. Final Amount: {Amount}");
    }

    /// <summary>
    /// 获取攻击意图的总攻击次数
    /// </summary>
    private int GetAttackIntentCount()
    {
        if (!Owner.IsMonster || Owner.Monster?.NextMove == null)
        {
            return 0;
        }

        var intents = Owner.Monster.NextMove.Intents;
        if (intents == null)
        {
            return 0;
        }

        int totalAttacks = 0;
        foreach (var intent in intents)
        {
            if (intent is AttackIntent attackIntent)
            {
                totalAttacks += attackIntent.Repeats;
                GD.Print($"[ScorchPower] Found attack intent: {intent.GetType().Name}, Repeats: {attackIntent.Repeats}");
            }
        }

        return totalAttacks;
    }

    /// <summary>
    /// 触发单次灼热伤害
    /// </summary>
    private async Task TriggerSingleScorchDamage(int attackNumber, string context = "")
    {
        var choiceContext = new ThrowingPlayerChoiceContext();
        
        try
        {
            string logContext = string.IsNullOrEmpty(context) ? $"Attack #{attackNumber}" : $"{context}";
            GD.Print($"[ScorchPower] {logContext}: Dealing {Amount} damage to {Owner.Name}...");

            // 造成伤害（伤害值 = 当前层数）
            await CreatureCmd.Damage(
                choiceContext,
                Owner,
                Amount,
                ValueProp.Unblockable | ValueProp.Unpowered,
                (Creature?)null,
                (CardModel?)null
            );

            // 如果目标还活着，减少层数
            if (Owner.IsAlive && Amount > 0)
            {
                await PowerCmd.Decrement(this);
            }
        }
        catch (System.Exception ex)
        {
            GD.PrintErr($"[ScorchPower] Error during attack #{attackNumber}: {ex.Message}");
            if (Amount > 0)
            {
                try { await PowerCmd.Decrement(this); } catch { }
            }
        }
    }
}
