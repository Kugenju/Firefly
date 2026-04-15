using BaseLib.Utils;
using Firefly.Scripts.CardPools;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Firefly.Scripts.Cards;

/// <summary>
/// 晖长石的烟火 - 普通技能牌
/// 获得1点能量。抽1张牌。升级：获得2点能量。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class SpheneFireworks : CardModel
{
    public SpheneFireworks() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => System.Array.Empty<DynamicVar>();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner?.Creature == null) return;

        // 获得能量
        int energyGain = IsUpgraded ? 2 : 1;
        await PlayerCmd.GainEnergy(energyGain, Owner);

        // 抽牌
        await CardPileCmd.Draw(choiceContext, 1, Owner, true);
    }

    protected override void OnUpgrade()
    {
        // 升级效果在 OnPlay 中处理
    }
}
