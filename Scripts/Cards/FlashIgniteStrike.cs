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
/// 闪燃打击 - 萤火攻击牌
/// 
/// 稀有度：Common（普通）
/// 费用：1
/// 类型：Attack（攻击）
/// 目标：AnyEnemy（任意敌人）
/// 
/// 效果：造成 !D! 点伤害。
/// 萤火：激发时效果翻倍，耗能-1
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class FlashIgniteStrike : CardModel
{
    public FlashIgniteStrike() 
        : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(8m, ValueProp.Move)
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null) return;

        // 检查是否被激发
        int multiplier = FireflyIgnitionManager.GetEffectMultiplier(this);
        
        // 获取当前伤害值
        int damage = (int)(DynamicVars.Damage.BaseValue * multiplier);

        // 造成伤害
        await CreatureCmd.Damage(
            choiceContext,
            cardPlay.Target,
            damage,
            ValueProp.Move,
            Owner?.Creature,
            this
        );
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4m);  // 8->12
    }
}
