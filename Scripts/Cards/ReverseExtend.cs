using BaseLib.Utils;
using Firefly.Scripts.CardPools;
using Firefly.Powers;
using System.Collections.Generic;
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
/// 反向延申 - 普通技能牌
/// 选择一名敌人使其格挡可以保留至下一个回合。升级：目标获得2层格挡保留。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class ReverseExtend : CardModel
{
    public ReverseExtend() : base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => System.Array.Empty<DynamicVar>();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null) return;

        int retainTurns = IsUpgraded ? 2 : 1;
        await PowerCmd.Apply<ReverseExtendBlockRetainPower>(
            cardPlay.Target,
            retainTurns,
            Owner?.Creature,
            this
        );
    }

    protected override void OnUpgrade()
    {
        // 升级效果：目标获得2层格挡保留
    }
}
