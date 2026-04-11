using BaseLib.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firefly.Scripts.CardPools;
using Firefly.Scripts.Keywords;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Firefly.Scripts.Cards;

/// <summary>
/// 完全燃烧 - 流萤的终结技
/// 
/// 稀有度：Rare（稀有）
/// 费用：2（升级后1）
/// 类型：Skill（技能）
/// 目标：Self（自身）
/// 
/// 效果：激发手中所有萤火牌。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class CompleteCombustionCard : CardModel
{
    public CompleteCombustionCard() 
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
        // 激发手中所有萤火牌
        if (Owner?.PlayerCombatState?.Hand?.Cards != null)
        {
            var fireflyCards = Owner.PlayerCombatState.Hand.Cards
                .Where(c => FireflyCardRegistry.IsFireflyCard(c))
                .ToList();

            foreach (var card in fireflyCards)
            {
                FireflyIgnitionManager.IgniteCard(card);
            }
        }

        await Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
        // 升级后费用变为1
        // 使用 SetThisCombat 或 SetUntilPlayed
        EnergyCost.SetThisCombat(1);
    }
}
