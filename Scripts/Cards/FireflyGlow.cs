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

    // TODO: 实现生命值低于10点才能打出的限制
    // 需要找到正确的条件打出API

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var combatState = Owner?.Creature?.CombatState;
        if (combatState == null || Owner?.Creature == null) return;

        // TODO: 计算已失去的生命值
        int lostHealth = 30; // 默认值
        int damage = lostHealth;

        // 对所有敌人造成伤害
        foreach (var enemy in combatState.HittableEnemies)
        {
            if (enemy.IsAlive)
            {
                await CreatureCmd.Damage(
                    choiceContext,
                    enemy,
                    damage,
                    ValueProp.Move,
                    Owner.Creature,
                    this
                );
            }
        }
    }

    protected override void OnUpgrade()
    {
        // 升级效果在 GetUnplayableReason 中处理
    }
}
