using BaseLib.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firefly.Scripts.CardPools;
using Firefly.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Firefly.Scripts.Cards;

/// <summary>
/// 格拉默！格拉默！ - 萤火专属技能牌（罕见）
///
/// 稀有度：Uncommon（罕见）
/// 费用：1（激发后0）
/// 类型：Skill（技能）
/// 目标：Self（自身）
///
/// 效果：获得15点格挡。下回合获得2点能量。
/// 激发：获得30点格挡，下回合获得4点能量，费用0。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class FireflyShield : FireflyCard
{
    public FireflyShield()
        : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new BlockVar(15m, ValueProp.Move)
    };

    protected override async Task OnFireflyPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner?.Creature == null) return;

        // 检查是否被激发
        int multiplier = FireflyIgnitionManager.GetEffectMultiplier(this);

        // 计算格挡值
        decimal blockAmount = DynamicVars.Block.BaseValue * multiplier;

        // 获得格挡
        await CreatureCmd.GainBlock(Owner.Creature, blockAmount, ValueProp.Move, cardPlay, false);

        // 下回合获得能量
        int nextTurnEnergy = 2 * multiplier;
        if (nextTurnEnergy > 0)
        {
            await PowerCmd.Apply<FireflyNextTurnEnergyPower>(Owner.Creature, nextTurnEnergy, Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(5m);  // 15->20
    }
}
