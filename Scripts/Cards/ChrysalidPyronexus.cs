using BaseLib.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Firefly.Scripts.CardPools;

namespace Firefly.Scripts.Cards;

/// <summary>
/// 女王低语 - 普通技能牌
/// 给予2层虚弱，给予2层易伤。消耗。升级：去除消耗。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class ChrysalidPyronexus : CardModel
{
    public ChrysalidPyronexus()
        : base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy, false)  // 不在构造函数中设置消耗
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => System.Array.Empty<DynamicVar>();

    /// <summary>
    /// 添加消耗关键词（未升级时）
    /// </summary>
    public override IEnumerable<CardKeyword> CanonicalKeywords
    {
        get
        {
            // 未升级时添加消耗关键词
            if (!IsUpgraded)
            {
                return new[] { CardKeyword.Exhaust };
            }
            return System.Array.Empty<CardKeyword>();
        }
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target != null)
        {
            // 给予2层虚弱
            await PowerCmd.Apply<MegaCrit.Sts2.Core.Models.Powers.WeakPower>(
                cardPlay.Target,
                2,
                Owner?.Creature,
                this
            );

            // 给予2层易伤
            await PowerCmd.Apply<MegaCrit.Sts2.Core.Models.Powers.VulnerablePower>(
                cardPlay.Target,
                2,
                Owner?.Creature,
                this
            );
        }
    }

    protected override void OnUpgrade()
    {
        // 升级后通过 CanonicalKeywords 不再返回 CardKeyword.Exhaust
        // 消耗属性自动去除
    }
}
