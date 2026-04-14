using BaseLib.Abstracts;
using BaseLib.Utils;
using Firefly.Scripts;
using Firefly.Scripts.Keywords;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Firefly.Powers;

/// <summary>
/// 完全燃烧状态 - 流萤的核心能力
/// 
/// 效果：
/// - 获得时：激发手牌中所有萤火牌
/// - 每当抽到萤火牌时：自动激发该牌
/// </summary>
[Pool(typeof(FireflyPowers))]
public class CompleteCombustionPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    /// <summary>
    /// 本地化定义
    /// </summary>
    public override List<(string, string)> Localization => new PowerLoc(
        Title: "完全燃烧",
        Description: "激发所有手牌中的萤火牌。每当你抽到萤火牌时，将其激发。",
        SmartDescription: "激发所有手牌中的萤火牌。每当你抽到萤火牌时，将其激发。"
    );

    /// <summary>
    /// Power 被应用后触发：激发所有手牌中的萤火牌
    /// </summary>
    public override async Task AfterApplied(Creature applier, CardModel cardSource)
    {
        await base.AfterApplied(applier, cardSource);
        IgniteAllFireflyCardsInHand();
    }

    /// <summary>
    /// 卡牌被抽到后触发：如果是萤火牌则激发
    /// </summary>
    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        await base.AfterCardDrawn(choiceContext, card, fromHandDraw);
        
        // 如果抽到的是萤火牌，激发它
        if (FireflyCardRegistry.IsFireflyCard(card))
        {
            FireflyIgnitionManager.IgniteCard(card);
        }
    }

    /// <summary>
    /// 激发手中所有萤火牌
    /// </summary>
    private void IgniteAllFireflyCardsInHand()
    {
        if (Owner?.Player?.PlayerCombatState?.Hand?.Cards != null)
        {
            var fireflyCards = Owner.Player.PlayerCombatState.Hand.Cards
                .Where(c => FireflyCardRegistry.IsFireflyCard(c))
                .ToList();

            foreach (var card in fireflyCards)
            {
                FireflyIgnitionManager.IgniteCard(card);
            }
        }
    }
}
