using BaseLib.Utils;
using Firefly.Scripts.CardPools;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Firefly.Powers;

namespace Firefly.Scripts.Cards;

/// <summary>
/// 母虫之怒 (Mother's Wrath) - 稀有能力卡
/// 
/// "觉醒的母虫..."
/// 
/// 每当你使敌人的格挡降低，对其施加1层灼热。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class MotherWrath : CardModel
{
    // 追踪上一个回合的格挡值
    [System.NonSerialized]
    private Dictionary<Creature, int> _lastBlockValues = new();

    public MotherWrath() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self, false)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner?.Creature == null)
        {
            GD.PrintErr("[MotherWrath] Owner is null!");
            return;
        }

        GD.Print("[MotherWrath] Power applied!");
        
        // 初始化当前所有敌人的格挡值
        var combatState = Owner.Creature.CombatState;
        if (combatState != null)
        {
            foreach (var creature in combatState.Creatures)
            {
                if (creature.IsMonster)
                {
                    _lastBlockValues[creature] = creature.Block;
                }
            }
        }

        await Task.CompletedTask;
    }

    /// <summary>
    /// 在卡牌打出后监听格挡变化
    /// 使用 AfterCardPlayed 来检测格挡降低
    /// </summary>
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 检查所有敌人的格挡变化
        var combatState = Owner?.Creature?.CombatState;
        if (combatState == null) return;

        foreach (var creature in combatState.Creatures)
        {
            if (!creature.IsMonster || !creature.IsAlive) continue;

            int currentBlock = creature.Block;
            
            // 如果之前记录过这个敌人的格挡
            if (_lastBlockValues.TryGetValue(creature, out int lastBlock))
            {
                int blockReduced = lastBlock - currentBlock;
                
                // 如果格挡降低了，施加灼热
                if (blockReduced > 0)
                {
                    GD.Print($"[MotherWrath] {creature.Name} block reduced by {blockReduced}, applying scorch...");
                    
                    await PowerCmd.Apply<ScorchPower>(
                        creature,
                        blockReduced,
                        Owner?.Creature,
                        this
                    );
                }
            }
            
            // 更新记录的格挡值
            _lastBlockValues[creature] = currentBlock;
        }

        await Task.CompletedTask;
    }

    /// <summary>
    /// 回合开始时更新格挡记录
    /// </summary>
    public override Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        // 记录当前所有敌人的格挡
        _lastBlockValues.Clear();
        foreach (var creature in combatState.Creatures)
        {
            if (creature.IsMonster)
            {
                _lastBlockValues[creature] = creature.Block;
            }
        }
        
        return Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
        // 升级效果：降低格挡时同时获得等量格挡
        // 这个效果需要在实际触发逻辑中实现
    }
}
