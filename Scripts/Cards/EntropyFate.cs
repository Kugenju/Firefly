using BaseLib.Utils;
using Firefly.Powers;
using Firefly.Scripts.CardPools;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Firefly.Scripts.Cards;

/// <summary>
/// 熵减宿命 - 普通攻击牌
/// 给予所有敌人6层格挡。对所有有格挡的敌人赋予裂解。升级：给予8层格挡。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class EntropyFate : CardModel
{
    public EntropyFate() : base(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => System.Array.Empty<DynamicVar>();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var combatState = Owner?.Creature?.CombatState;
        if (combatState == null) return;

        int blockAmount = IsUpgraded ? 8 : 6;

        // 给予所有敌人格挡并赋予裂解
        foreach (var enemy in combatState.HittableEnemies)
        {
            if (enemy.IsAlive)
            {
                GD.Print($"[EntropyFate] Giving {blockAmount} block to {enemy.Name}");
                
                // 先应用裂解源Power
                await PowerCmd.Apply<DissolutionSourcePower>(
                    enemy,
                    blockAmount,
                    Owner?.Creature,
                    this
                );

                // 给予格挡
                await CreatureCmd.GainBlock(
                    enemy,
                    blockAmount,
                    ValueProp.Move,
                    cardPlay,
                    false
                );

                // 赋予裂解标记
                await PowerCmd.Apply<DissolutionPower>(
                    enemy,
                    1,
                    Owner?.Creature,
                    this
                );
            }
        }
    }

    protected override void OnUpgrade()
    {
        // 升级效果在 OnPlay 中处理
    }
}
