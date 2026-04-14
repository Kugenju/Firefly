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
/// 火萤II型·强袭 - 流萤的能力牌
/// 
/// 稀有度：Uncommon（罕见）
/// 费用：1
/// 类型：Power（能力）
/// 目标：Self（自身）
/// 
/// 效果：进入装甲燃烧状态。
/// 装甲燃烧：受到的伤害减少5点。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class ArmoredCombustionCard : CardModel
{
    public ArmoredCombustionCard() 
        : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => System.Array.Empty<DynamicVar>();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner?.Creature == null) return;

        // 施加装甲燃烧状态
        await PowerCmd.Apply<ArmoredCombustionPower>(Owner.Creature, 5, Owner.Creature, this);

        await Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
        // 升级后减伤数值提升
        // 实际效果在 Power 中，这里可以添加升级后的数值变化
    }
}
