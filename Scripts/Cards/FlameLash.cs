using BaseLib.Utils;
using Firefly.Powers;
using Firefly.Scripts.CardPools;
using Godot;
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
/// 火萤斩击 - 普通攻击牌（卡池）
/// 造成 7/9 点伤害两次，施加 2/3 层灼热
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class FlameLash : CardModel
{
    public FlameLash() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new DamageVar(7m, ValueProp.Move)
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target != null)
        {
            // 先施加灼热（简化测试）
            int scorchAmount = IsUpgraded ? 3 : 2;
            GD.Print($"[FlameLash] Applying {scorchAmount} Scorch to {cardPlay.Target.Name}");
            
            // 使用 null 作为 applier（卡片本身作为来源）
            var result = await PowerCmd.Apply<ScorchPower>(
                cardPlay.Target,
                scorchAmount,
                null,
                this
            );
            
            GD.Print($"[FlameLash] ScorchPower applied: {result != null}");

            // 第一次攻击
            await DamageCmd.Attack(7m)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .Execute(choiceContext);

            // 第二次攻击
            await DamageCmd.Attack(7m)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .Execute(choiceContext);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
    }
}
