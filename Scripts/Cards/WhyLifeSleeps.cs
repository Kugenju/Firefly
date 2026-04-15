using BaseLib.Utils;
using Firefly.Scripts.CardPools;
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
/// 生命因何而沉睡 - 稀有技能牌
/// 生命值上限减少1点。本场战斗每回合获得2点能量。升级：获得3点能量。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class WhyLifeSleeps : CardModel
{
    public WhyLifeSleeps() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => System.Array.Empty<DynamicVar>();

    private const int MAX_HEALTH_REDUCTION = 1;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner?.Creature == null) return;

        // TODO: 减少生命值上限
        // 这需要修改玩家的最大生命值

        // 应用每回合获得能量的能力
        int energyAmount = IsUpgraded ? 3 : 2;
        // TODO: 应用能力
        await Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
        // 升级效果在 OnPlay 中处理
    }
}
