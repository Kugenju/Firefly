using BaseLib.Utils;
using Firefly.Powers;
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
/// 热能护盾 - 普通技能牌
/// 获得 8/11 点格挡，获得2层灼热（给予自己，下回合生效）
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class HeatShield : CardModel
{
    public HeatShield() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new BlockVar(8m, ValueProp.Move)
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner?.Creature != null)
        {
            int scorchAmount = IsUpgraded ? 3 : 2; // 升级后施加3层灼热

            // 获得格挡
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay, false);

            // 给自己施加灼热（战术性自伤，配合某些卡牌机制）
            await PowerCmd.Apply<ScorchPower>(
                Owner.Creature,
                scorchAmount,
                Owner.Creature,
                this
            );
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(2m);
    }
}
