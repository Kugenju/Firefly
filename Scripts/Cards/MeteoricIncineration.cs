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
/// 焦土陨击 - AOE攻击牌（初始卡组）
/// 对所有敌人造成 8/12 点伤害，施加3层灼热
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class MeteoricIncineration : CardModel
{
    public MeteoricIncineration() 
        : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new DamageVar(8m, ValueProp.Move)
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 获取所有可攻击的敌人
        var combatState = cardPlay.Card.Owner?.Creature?.CombatState;
        if (combatState == null) return;

        var enemies = combatState.HittableEnemies;
        int scorchAmount = IsUpgraded ? 4 : 3; // 升级后施加4层灼热

        // 对所有敌人施加灼热
        foreach (var enemy in enemies)
        {
            if (enemy.IsAlive)
            {
                await PowerCmd.Apply<ScorchPower>(
                    enemy,
                    scorchAmount,
                    cardPlay.Card.Owner?.Creature,
                    this
                );
            }
        }

        // 对所有敌人造成伤害
        foreach (var enemy in enemies)
        {
            if (enemy.IsAlive)
            {
                await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                    .FromCard(this)
                    .Targeting(enemy)
                    .Execute(choiceContext);
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4m);
    }
}
