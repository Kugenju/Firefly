using BaseLib.Utils;
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
/// 牺牲 - 普通攻击牌
/// 失去1点生命值。对所有敌人造成12点伤害。升级：造成18点伤害。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class SacrificeStrike : CardModel
{
    public SacrificeStrike() : base(1, CardType.Attack, CardRarity.Common, TargetType.AllEnemies, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new DamageVar(12m, ValueProp.Move)
    };

    private const int HEALTH_COST = 1;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 先失去生命值
        if (Owner?.Creature != null)
        {
            await CreatureCmd.Damage(
                choiceContext,
                Owner.Creature,
                HEALTH_COST,
                ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move,
                null,
                this
            );
        }

        // 对所有敌人造成伤害
        var combatState = Owner?.Creature?.CombatState;
        if (combatState != null)
        {
            int damage = (int)DynamicVars.Damage.BaseValue;
            foreach (var enemy in combatState.HittableEnemies)
            {
                if (enemy.IsAlive)
                {
                    await DamageCmd.Attack(damage)
                        .FromCard(this)
                        .Targeting(enemy)
                        .Execute(choiceContext);
                }
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(6m); // 12->18
    }
}
