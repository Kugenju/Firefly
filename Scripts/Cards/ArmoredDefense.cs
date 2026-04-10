using BaseLib.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firefly.Scripts.CardPools;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Firefly.Scripts.Cards;

/// <summary>
/// 装甲防御 - 萤火防御牌
/// 
/// 稀有度：Common（普通）
/// 费用：1
/// 类型：Skill（技能）
/// 目标：Self（自身）
/// 
/// 效果：获得 !B! 点格挡。
/// 萤火：激发时效果翻倍，耗能-1
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class ArmoredDefense : CardModel
{
    public ArmoredDefense() 
        : base(1, CardType.Skill, CardRarity.Common, TargetType.Self, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new BlockVar(8m, ValueProp.Move)
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 检查是否被激发
        int multiplier = FireflyIgnitionManager.GetEffectMultiplier(this);
        
        // 获取当前格挡值
        int block = (int)(DynamicVars.Block.BaseValue * multiplier);

        // 获得格挡
        if (Owner?.Creature != null)
        {
            await CreatureCmd.GainBlock(
                Owner.Creature,
                block,
                ValueProp.Move,
                cardPlay,
                false
            );
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(4m);  // 8->12
    }
}
