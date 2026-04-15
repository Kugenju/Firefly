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
/// 紧急修复 - 普通技能牌
/// 失去2点生命值。获得8点格挡。抽1张牌。升级：获得12点格挡。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class EmergencyRepair : CardModel
{
    public EmergencyRepair() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new BlockVar(8m, ValueProp.Move)
    };

    private const int HEALTH_COST = 2;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner?.Creature == null) return;

        // 先失去生命值
        await CreatureCmd.Damage(
            choiceContext,
            Owner.Creature,
            HEALTH_COST,
            ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move,
            null,
            this
        );

        // 获得格挡
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay, false);

        // TODO: 抽牌
        await Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(4m); // 8->12
    }
}
