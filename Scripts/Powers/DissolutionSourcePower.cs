using System.Collections.Generic;
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
using MegaCrit.Sts2.Core.ValueProps;

namespace Firefly.Powers;

/// <summary>
/// 裂解源 (Dissolution Source) - 核心机制Power
/// 
/// 负责追踪敌人本回合获得的格挡总数，并在格挡被打破时触发裂解伤害。
/// 这是一个内部Buff，显示当前累积的格挡数。
/// </summary>
public class DissolutionSourcePower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    // 使用 Counter 类型显示数字
    public override PowerStackType StackType => PowerStackType.Counter;

    public override List<(string, string)> Localization => new PowerLoc(
        Title: "裂解源",
        Description: "本回合获得{Amount}点格挡。格挡被击破时受到等于层数的反馈伤害。",
        SmartDescription: "本回合获得{Amount}点格挡。格挡被击破时受到等于层数的反馈伤害。"
    );

    /// <summary>
    /// 回合开始时重置计数
    /// </summary>
    public override Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side == Owner.Side)
        {
            // 如果还有未触发的计数，说明上回合格挡没被打破，直接清零
            if (Amount > 0)
            {
                GD.Print($"[DissolutionSourcePower] {Owner.Name} turn start, clearing old count: {Amount}");
            }
            Amount = 0;
        }
        return Task.CompletedTask;
    }

    /// <summary>
    /// 格挡被打破时触发裂解伤害
    /// </summary>
    public override async Task AfterBlockBroken(Creature creature)
    {
        if (creature != Owner)
            return;

        GD.Print($"[DissolutionSourcePower] {Owner.Name} block broken! Current count: {Amount}");

        // 只有在本回合获得过格挡时才触发
        if (Amount <= 0)
        {
            GD.Print($"[DissolutionSourcePower] {Owner.Name} no block to dissolve.");
            return;
        }

        // 保存当前伤害值
        int damage = Amount;

        // 触发裂解伤害
        await TriggerDissolutionDamage(damage);

        // 给敌人添加裂解标记（用于后续卡牌效果）
        if (Owner.IsAlive)
        {
            var applier = Applier ?? Owner;
            await PowerCmd.Apply<DissolutionPower>(Owner, 1, applier, null);
        }

        // 触发后重置计数
        Amount = 0;
    }

    /// <summary>
    /// 触发裂解伤害
    /// </summary>
    private async Task TriggerDissolutionDamage(int damage)
    {
        var choiceContext = new ThrowingPlayerChoiceContext();
        
        try
        {
            var dealer = Applier ?? Owner;
            GD.Print($"[DissolutionSourcePower] Dealing {damage} dissolution damage to {Owner.Name}");

            await CreatureCmd.Damage(
                choiceContext,
                Owner,
                damage,
                ValueProp.Unblockable | ValueProp.Unpowered,
                dealer,
                null
            );

            GD.Print($"[DissolutionSourcePower] Dissolution damage dealt successfully.");
        }
        catch (System.Exception ex)
        {
            GD.PrintErr($"[DissolutionSourcePower] Error: {ex.Message}");
        }
    }
}
