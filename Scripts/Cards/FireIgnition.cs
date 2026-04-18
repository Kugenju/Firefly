using BaseLib.Utils;
using Firefly.Scripts.CardPools;
using Firefly.Scripts.Keywords;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Firefly.Scripts.Cards;

/// <summary>
/// 萤火燎原（快速点燃）- 罕见技能牌
/// 选择手牌中一张牌，使其获得萤火属性。抽1张牌。升级：抽2张牌。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class FireIgnition : CardModel
{
    public FireIgnition() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => System.Array.Empty<DynamicVar>();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var owner = Owner;
        if (owner == null)
        {
            return;
        }

        var prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1);
        var selectedCard = (await CardSelectCmd.FromHand(
                choiceContext,
                owner,
                prefs,
                card => !card.Keywords.Contains(FireflyKeywords.Firefly),
                this))
            .FirstOrDefault();

        if (selectedCard != null)
        {
            CardCmd.ApplyKeyword(selectedCard, new[] { FireflyKeywords.Firefly });
        }

        // 抽牌
        int drawCount = IsUpgraded ? 2 : 1;
        await CardPileCmd.Draw(choiceContext, drawCount, owner, true);
    }

    protected override void OnUpgrade()
    {
        // 升级效果在 OnPlay 中处理
    }
}
