using System.Collections.Generic;
using System.Threading.Tasks;
using Firefly.Scripts.CardPools;
using Firefly.Scripts.Keywords;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Firefly.Scripts.Cards;

/// <summary>
/// 萤火卡牌基类 - 所有萤火牌继承此类
/// 自动处理激发状态的视觉效果和清除
/// </summary>
public abstract class FireflyCard : CardModel
{
    protected FireflyCard(int energyCost, CardType cardType, CardRarity rarity, TargetType targetType, bool isExhaust = false)
        : base(energyCost, cardType, rarity, targetType, isExhaust)
    {
    }

    /// <summary>
    /// 萤火关键词（所有萤火牌都有）
    /// </summary>
    public override IEnumerable<CardKeyword> CanonicalKeywords => new[]
    {
        FireflyKeywords.Firefly
    };

    /// <summary>
    /// 卡牌打出时先执行效果，再清除激发状态
    /// </summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 先执行子类的具体逻辑（此时激发状态还在，伤害会翻倍）
        await OnFireflyPlay(choiceContext, cardPlay);
        
        // 效果执行完成后再清除激发状态
        FireflyIgnitionManager.ClearIgnition(this);
    }

    /// <summary>
    /// 子类重写此方法实现具体的卡牌效果
    /// </summary>
    protected abstract Task OnFireflyPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay);
}
