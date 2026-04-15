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
/// 静默的星河 - 稀有攻击牌
/// 对所有拥有裂解的敌人造成15点伤害。每有一个裂解敌人，获得3点格挡。升级：造成20点伤害。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class SilentStars : CardModel
{
    public SilentStars() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new DamageVar(15m, ValueProp.Move)
    };

    private const int BLOCK_PER_ENEMY = 3;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var combatState = Owner?.Creature?.CombatState;
        if (combatState == null) return;

        int blockPerEnemy = IsUpgraded ? 4 : BLOCK_PER_ENEMY;
        int enemiesWithDissolution = 0;

        // 对所有有裂解的敌人造成伤害
        foreach (var enemy in combatState.HittableEnemies)
        {
            if (enemy.IsAlive && enemy.Powers.Any(p => p is DissolutionPower))
            {
                enemiesWithDissolution++;

                await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                    .FromCard(this)
                    .Targeting(enemy)
                    .Execute(choiceContext);
            }
        }

        // 获得格挡
        if (enemiesWithDissolution > 0 && Owner?.Creature != null)
        {
            int totalBlock = enemiesWithDissolution * blockPerEnemy;
            await CreatureCmd.GainBlock(
                Owner.Creature,
                totalBlock,
                ValueProp.Move,
                cardPlay,
                false
            );
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(5m); // 15->20
    }
}
