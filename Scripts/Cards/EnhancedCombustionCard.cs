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
/// 火萤I型·推进 - 流萤的能力牌
/// 
/// 稀有度：Uncommon（罕见）
/// 费用：1
/// 类型：Power（能力）
/// 目标：Self（自身）
/// 
/// 效果：进入强化燃烧状态。
/// 强化燃烧：每回合多抽1张牌。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class EnhancedCombustionCard : CardModel
{
    public EnhancedCombustionCard() 
        : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => System.Array.Empty<DynamicVar>();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner?.Creature == null) return;

        // 施加强化燃烧状态
        await PowerCmd.Apply<EnhancedCombustionPower>(Owner.Creature, 1, Owner.Creature, this);

        await Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
        // 升级后费用变为0
        EnergyCost.SetThisCombat(0);
    }
}
