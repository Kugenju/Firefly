using BaseLib.Utils;
using Firefly.Powers;
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
/// 欧米伽爆破 - 稀有攻击牌
/// 消耗。对所有敌人造成20点伤害。施加5层灼热。升级：造成28点伤害。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class OmegaBlast : CardModel
{
    public OmegaBlast() : base(3, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies, false)
    {
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords => new[] { CardKeyword.Exhaust };

    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new DamageVar(20m, ValueProp.Move)
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var combatState = Owner?.Creature?.CombatState;
        if (combatState == null) return;

        int scorchAmount = IsUpgraded ? 7 : 5;

        // 对所有敌人施加灼热和伤害
        foreach (var enemy in combatState.HittableEnemies)
        {
            if (enemy.IsAlive)
            {
                await PowerCmd.Apply<ScorchPower>(
                    enemy,
                    scorchAmount,
                    Owner?.Creature,
                    this
                );

                await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                    .FromCard(this)
                    .Targeting(enemy)
                    .Execute(choiceContext);
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(8m); // 20->28
    }
}
