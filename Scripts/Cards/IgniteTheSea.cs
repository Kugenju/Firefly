using BaseLib.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firefly.Scripts.CardPools;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Firefly.Scripts.Cards;

/// <summary>
/// 点燃大海 - 流萤的终极宣言
/// 
/// 稀有度：Rare（稀有）
/// 费用：2（升级后1）
/// 类型：Skill（技能）
/// 目标：Self（自身）
/// 
/// 效果：激发手牌中所有萤火牌。
/// 台词："我将，点燃大海！"
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class IgniteTheSea : CardModel
{
    public IgniteTheSea() 
        : base(2, CardType.Skill, CardRarity.Rare, TargetType.Self, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => System.Array.Empty<DynamicVar>();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 获取手中的所有萤火牌
        if (Owner?.PlayerCombatState?.Hand?.Cards != null)
        {
            var fireflyCards = Owner.PlayerCombatState.Hand.Cards
                .Where(c => FireflyCardRegistry.IsFireflyCard(c))
                .ToList();

            // 激发所有萤火牌
            foreach (var card in fireflyCards)
            {
                FireflyIgnitionManager.IgniteCard(card);
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
