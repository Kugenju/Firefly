using BaseLib.Abstracts;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Firefly.Powers;

/// <summary>
/// 无视格挡 - 本回合攻击无视敌人格挡
/// Amount: 1 = 下一张攻击，-1 = 本回合所有攻击
///
/// 注意：此Power仅作为标记，实际的无视格挡效果需要在卡牌代码中检查并应用
/// </summary>
// Power auto-registered without Pool attribute
public class FireflyIgnoreBlockPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override List<(string, string)> Localization => new PowerLoc(
        Title: "偏时迸发",
        Description: "本回合你的下一张攻击牌无视敌人格挡。",
        SmartDescription: "本回合你的下一张攻击牌无视敌人格挡。"
    );

    /// <summary>
    /// 检查是否应该无视格挡，并消耗层数
    /// </summary>
    public bool ShouldIgnoreBlock()
    {
        if (Amount == 0)
        {
            return false;
        }

        // 如果不是本回合所有攻击（Amount != -1），则减少层数
        if (Amount > 0)
        {
            Amount--;
        }

        GD.Print($"[FireflyIgnoreBlockPower] Ignoring block. Remaining Amount: {Amount}");
        return true;
    }

    /// <summary>
    /// 回合结束时移除
    /// </summary>
    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (Owner.Side != side)
        {
            await PowerCmd.Remove(this);
        }
    }
}
