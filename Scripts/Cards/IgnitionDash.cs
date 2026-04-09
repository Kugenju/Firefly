using BaseLib.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Firefly.Scripts.CardPools;

namespace Firefly.Scripts.Cards;

/// <summary>
/// 点燃冲刺 - 普通攻击牌
/// 造成 8/11 点伤害
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class IgnitionDash : CardModel
{
    public IgnitionDash() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new DamageVar(8m, ValueProp.Move)
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}
