using BaseLib.Utils;
using Firefly.Scripts.CardPools;
using Firefly.Powers;
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
/// 时停 - 稀有技能牌
/// 本回合敌人不行动。你受到的所有伤害+5。消耗。升级：受到伤害+3。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class TimeHalt : CardModel
{
    public TimeHalt() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self, false)
    {
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords => new[] { CardKeyword.Exhaust };

    protected override IEnumerable<DynamicVar> CanonicalVars => System.Array.Empty<DynamicVar>();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var ownerCreature = Owner?.Creature;
        var combatState = ownerCreature?.CombatState;
        if (ownerCreature == null || combatState == null) return;

        // 本回合敌人不行动：使所有存活敌人眩晕
        foreach (var enemy in combatState.HittableEnemies)
        {
            if (enemy.IsAlive && enemy.IsMonster)
            {
                await CreatureCmd.Stun(enemy);
            }
        }

        // 本回合自身受到所有伤害增加
        int damageIncrease = IsUpgraded ? 3 : 5;
        await PowerCmd.Apply<TimeHaltBacklashPower>(
            ownerCreature,
            damageIncrease,
            ownerCreature,
            this
        );
    }

    protected override void OnUpgrade()
    {
        // 升级效果在 OnPlay 中处理（伤害+3 代替 +5）
    }
}
