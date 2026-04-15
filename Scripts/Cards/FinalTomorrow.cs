using BaseLib.Utils;
using Firefly.Scripts.CardPools;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Firefly.Scripts.Cards;

/// <summary>
/// 终竟的明天 - 罕见技能牌
/// 获得6点格挡。将手牌中所有萤火牌置入弃牌堆，每张抽1张牌。升级：抽2张。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class FinalTomorrow : CardModel
{
    public FinalTomorrow() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new BlockVar(6m, ValueProp.Move)
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner?.Creature == null) return;

        // 获得格挡
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay, false);

        // 获取手牌中的萤火牌
        var fireflyCards = Owner.PlayerCombatState.Hand.Cards
            .Where(c => FireflyCardRegistry.IsFireflyCard(c) && c != this)
            .ToList();

        int drawPerCard = IsUpgraded ? 2 : 1;
        int totalDraw = fireflyCards.Count * drawPerCard;

        // 将萤火牌置入弃牌堆
        foreach (var card in fireflyCards)
        {
            await CardCmd.Discard(choiceContext, card);
        }

        // 抽牌 - TODO: 需要找到从CardModel获取Player的正确方法
        await Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3m); // 6->9
    }
}
