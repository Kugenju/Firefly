using BaseLib.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firefly.Powers;
using Firefly.Scripts.CardPools;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Firefly.Scripts.Cards;

/// <summary>
/// 过载核心 - 普通能力牌
/// 
/// 稀有度：Common（普通）
/// 费用：2
/// 类型：Power（能力）
/// 目标：Self（自身）
/// 
/// 效果：每当你失去生命值，获得1层力量（持续1回合）。
/// 升级：每当你失去生命值，获得2层力量。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class OverloadCore : CardModel
{
    public OverloadCore() 
        : base(2, CardType.Power, CardRarity.Common, TargetType.Self, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => System.Array.Empty<DynamicVar>();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner?.Creature == null) return;

        // 施加强化过载核心能力
        await PowerCmd.Apply<OverloadCorePower>(Owner.Creature, 1, Owner.Creature, this);

        await Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
        // 升级效果在Power中处理
    }
}
