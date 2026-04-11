using BaseLib.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firefly.Scripts.CardPools;
using Firefly.Scripts.Keywords;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Firefly.Scripts.Cards;

/// <summary>
/// 海一直燃 - 让火焰蔓延
/// 
/// 稀有度：Rare（稀有）
/// 费用：2（升级后1）
/// 类型：Skill（技能）
/// 目标：Self（自身）
/// 
/// 效果：打出手牌中所有的萤火牌。
/// 台词："让火焰蔓延"
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class FlamesSpread : CardModel
{
    public FlamesSpread() 
        : base(2, CardType.Skill, CardRarity.Rare, TargetType.Self, false)
    {
    }

    // 萤火关键词
    public override IEnumerable<CardKeyword> CanonicalKeywords => new[]
    {
        FireflyKeywords.Firefly
    };

    protected override IEnumerable<DynamicVar> CanonicalVars => System.Array.Empty<DynamicVar>();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 获取手中的所有萤火牌
        if (Owner?.PlayerCombatState?.Hand?.Cards == null)
        {
            await Task.CompletedTask;
            return;
        }

        var fireflyCards = Owner.PlayerCombatState.Hand.Cards
            .Where(c => FireflyCardRegistry.IsFireflyCard(c) && c != this)
            .ToList();

        // 依次打出所有萤火牌
        foreach (var card in fireflyCards)
        {
            // 确保卡牌还在手牌中
            if (Owner.PlayerCombatState.Hand.Cards.Contains(card))
            {
                // 自动打出卡牌
                await CardCmd.AutoPlay(
                    choiceContext,
                    card,
                    null,  // 目标由卡牌自动选择
                    AutoPlayType.Default
                );
            }
        }

        await Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
        // 升级后费用从2减少到1
        EnergyCost.SetThisCombat(1);
    }
}
