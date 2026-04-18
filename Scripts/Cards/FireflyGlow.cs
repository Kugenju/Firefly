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
/// 飞萤之火 - 稀有攻击牌
/// 生命值低于10点时才能打出。对所有敌人造成等于已失去生命值的伤害。升级：7点生命阈值。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class FireflyGlow : CardModel
{
    public FireflyGlow() : base(0, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => System.Array.Empty<DynamicVar>();

    private const int HEALTH_THRESHOLD = 10;
    private const int UPGRADED_THRESHOLD = 7;

    protected override bool IsPlayable
    {
        get
        {
            if (!base.IsPlayable)
            {
                return false;
            }

            var ownerCreature = Owner?.Creature;
            if (ownerCreature == null)
            {
                return false;
            }

            int threshold = IsUpgraded ? UPGRADED_THRESHOLD : HEALTH_THRESHOLD;
            return ownerCreature.CurrentHp < threshold;
        }
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var ownerCreature = Owner?.Creature;
        var combatState = ownerCreature?.CombatState;
        if (combatState == null || ownerCreature == null) return;

        int lostHealth = ownerCreature.MaxHp - ownerCreature.CurrentHp;
        int damage = lostHealth;

        // 对所有敌人造成伤害
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

    protected override void OnUpgrade()
    {
        // 升级效果在 GetUnplayableReason 中处理
    }
}
