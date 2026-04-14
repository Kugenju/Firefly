using BaseLib.Utils;
using Firefly.Scripts.CardPools;
using Firefly.Powers;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Firefly.Scripts.Cards;

/// <summary>
/// 偏时迸发 - 普通技能牌
/// 本回合你的下一张攻击牌无视敌人格挡。升级：本回合所有攻击牌无视格挡。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class TimeDelayBurst : CardModel
{
    public TimeDelayBurst() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => System.Array.Empty<DynamicVar>();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner?.Creature != null)
        {
            // 应用无视格挡的Power
            int stacks = IsUpgraded ? -1 : 1;  // -1 表示本回合所有攻击，1 表示下一张攻击
            await PowerCmd.Apply<FireflyIgnoreBlockPower>(
                Owner.Creature,
                stacks,
                Owner.Creature,
                this
            );
        }
    }

    protected override void OnUpgrade()
    {
        // 升级效果在 OnPlay 中处理
    }
}
