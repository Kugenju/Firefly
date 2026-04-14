using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Firefly.Powers;

/// <summary>
/// 强化燃烧 - 每回合额外抽牌
/// 
/// 效果：每回合多抽1张牌
/// </summary>
[Pool(typeof(FireflyPowers))]
public class EnhancedCombustionPower : CustomPowerModel
{
    // 额外抽牌数量
    private const int EXTRA_DRAW = 1;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    /// <summary>
    /// 本地化定义
    /// </summary>
    public override List<(string, string)> Localization => new PowerLoc(
        Title: "强化燃烧",
        Description: "每回合多抽{0}张牌。",
        SmartDescription: "每回合多抽{0}张牌。"
    );

    /// <summary>
    /// 回合开始时：额外抽牌
    /// </summary>
    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        // 只在我们所在的阵营回合开始时触发
        if (side != Owner.Side)
        {
            return;
        }

        // 额外抽牌
        if (Owner?.Player != null)
        {
            var choiceContext = new ThrowingPlayerChoiceContext();
            await CardPileCmd.Draw(choiceContext, EXTRA_DRAW, Owner.Player, true);
        }
    }
}
