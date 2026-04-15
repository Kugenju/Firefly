using BaseLib.Utils;
using Firefly.Scripts.CardPools;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Firefly.Scripts.Cards;

/// <summary>
/// 无梦的长夜 - 稀有技能牌
/// 将弃牌堆中所有萤火牌返回手牌。获得等于返回数量×2的格挡。升级：×3。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class DreamlessNight : CardModel
{
    public DreamlessNight() : base(2, CardType.Skill, CardRarity.Rare, TargetType.Self, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => System.Array.Empty<DynamicVar>();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner?.Creature == null) return;

        // 获取弃牌堆中的萤火牌
        var fireflyCards = Owner.PlayerCombatState.DiscardPile.Cards
            .Where(c => FireflyCardRegistry.IsFireflyCard(c))
            .ToList();

        int returnedCount = fireflyCards.Count;
        int blockPerCard = IsUpgraded ? 3 : 2;
        int totalBlock = returnedCount * blockPerCard;

        // 将萤火牌返回手牌 - 暂无法实现
        // 已尝试的API（均不存在）：
        // - CardCmd.MoveToHand(card)
        // - CardCmd.ReturnToHand(card, Owner)
        // - CardCmd.Retrieve(card, Owner)
        // - CardCmd.Recycle(choiceContext, card)
        // - CardCmd.Restore(choiceContext, card)
        // - CardPileCmd.MoveToHand(card, Owner)
        // - CardPileCmd.Move(card, from, to)
        // - Owner.PlayerCombatState.Hand.AddCard(card)
        // 可能需要通过Harmony Patch或反编译BaseLib找到正确API
        foreach (var card in fireflyCards)
        {
            // 暂时无法实现从弃牌堆移动卡牌到手牌
        }

        // 获得格挡
        if (totalBlock > 0)
        {
            await CreatureCmd.GainBlock(Owner.Creature, totalBlock, ValueProp.Move, cardPlay, false);
        }
    }

    protected override void OnUpgrade()
    {
        // 升级效果在 OnPlay 中处理
    }
}
