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
/// 燃烧意志 - 罕见技能牌
/// 失去4点生命值。本回合获得2层力量。升级：获得3层力量。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class BurningWill : CardModel
{
    public BurningWill() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => System.Array.Empty<DynamicVar>();

    private const int HEALTH_COST = 4;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner?.Creature == null) return;

        // 失去生命值
        await CreatureCmd.Damage(
            choiceContext,
            Owner.Creature,
            HEALTH_COST,
            ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move,
            null,
            this
        );

        // 获得临时力量
        int strengthAmount = IsUpgraded ? 3 : 2;
        await PowerCmd.Apply<MegaCrit.Sts2.Core.Models.Powers.StrengthPower>(
            Owner.Creature,
            strengthAmount,
            Owner.Creature,
            this,
            false // 临时力量
        );
    }

    protected override void OnUpgrade()
    {
        // 升级效果在 OnPlay 中处理
    }
}
