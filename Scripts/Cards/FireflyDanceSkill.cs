using BaseLib.Utils;
using Firefly.Scripts.CardPools;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Firefly.Scripts.Cards;

/// <summary>
/// 萤火之舞 - 罕见技能牌
/// 抽2张牌。本回合你打出的下一张攻击牌费用为0。升级：抽3张牌。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class FireflyDanceSkill : CardModel
{
    public FireflyDanceSkill() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => System.Array.Empty<DynamicVar>();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner?.Creature == null) return;

        // 抽牌
        int drawCount = IsUpgraded ? 3 : 2;
        await CardPileCmd.Draw(choiceContext, drawCount, Owner, true);

        // TODO: 下一张攻击牌费用为0的效果
    }

    protected override void OnUpgrade()
    {
        // 升级效果在 OnPlay 中处理
    }
}
