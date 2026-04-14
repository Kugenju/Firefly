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
/// 能量超载 - 普通技能牌
/// 获得2点能量。赋予自身2层灼热。升级：获得3点能量。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class EnergyOverload : CardModel
{
    public EnergyOverload() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => System.Array.Empty<DynamicVar>();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner != null)
        {
            // 获得能量
            int energyGain = IsUpgraded ? 3 : 2;
            await PlayerCmd.GainEnergy(energyGain, Owner);

            // 赋予自身灼热
            if (Owner.Creature != null)
            {
                int scorchAmount = 2;
                await PowerCmd.Apply<ScorchPower>(
                    Owner.Creature,
                    scorchAmount,
                    Owner.Creature,
                    this
                );
            }
        }
    }

    protected override void OnUpgrade()
    {
        // 升级效果在 OnPlay 中处理
    }
}
