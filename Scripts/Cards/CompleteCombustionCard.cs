using BaseLib.Utils;
using Firefly.Scripts.CardPools;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Firefly.Scripts.Cards;

/// <summary>
/// 完全燃烧 - 流萤的终结技
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class CompleteCombustionCard : CardModel
{
    public CompleteCombustionCard() 
        : base(3, CardType.Skill, CardRarity.Rare, TargetType.Self, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new DamageVar(0m, ValueProp.Move)  // 占位，实际效果需要实现
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
    }
}
