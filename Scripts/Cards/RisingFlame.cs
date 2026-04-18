using BaseLib.Utils;
using Firefly.Powers;
using Firefly.Scripts.CardPools;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Firefly.Scripts.Cards;

/// <summary>
/// 升腾之焰 - 罕见攻击牌
/// 造成5点伤害。每层灼热使伤害+2。升级：每层灼热使伤害+3。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class RisingFlame : CardModel
{
    public RisingFlame() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new DamageVar(5m, ValueProp.Move)
    };

    private const int DAMAGE_PER_SCORCH = 2;
    private const int UPGRADED_BONUS = 3;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null) return;

        int baseDamage = (int)DynamicVars.Damage.BaseValue;
        int bonusPerScorch = IsUpgraded ? UPGRADED_BONUS : DAMAGE_PER_SCORCH;
        
        // 获取目标身上的灼热层数
        int scorchStacks = 0;
        var scorchPower = cardPlay.Target.Powers.FirstOrDefault(p => p is ScorchPower);
        if (scorchPower != null)
        {
            scorchStacks = scorchPower.Amount;
        }

        int finalDamage = baseDamage + (scorchStacks * bonusPerScorch);

        await DamageCmd.Attack(finalDamage)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        // 升级效果在 OnPlay 中处理
    }
}
