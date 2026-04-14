using BaseLib.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firefly.Scripts.CardPools;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Firefly.Scripts.Cards;

/// <summary>
/// 荧火突刺 - 萤火专属攻击牌（罕见）
/// 
/// 稀有度：Uncommon（罕见）
/// 费用：2（激发后1）
/// 类型：Attack（攻击）
/// 目标：AnyEnemy（任意敌人）
/// 
/// 效果：造成15点伤害。获得2层临时力量。
/// 激发：造成30点伤害，获得4层临时力量，费用-1。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class FluorescentThrust : FireflyCard
{
    public FluorescentThrust() 
        : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new DamageVar(15m, ValueProp.Move)
    };

    protected override async Task OnFireflyPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null) return;

        // 检查是否被激发
        int multiplier = FireflyIgnitionManager.GetEffectMultiplier(this);
        
        // 计算伤害
        int damage = (int)DynamicVars.Damage.BaseValue * multiplier;
        
        // 计算临时力量
        int tempStrength = 2 * multiplier;

        // 造成伤害
        await DamageCmd.Attack(damage)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);

        // 获得临时力量 - 使用游戏内置的临时力量Power
        if (Owner?.Creature != null && tempStrength > 0)
        {
            await PowerCmd.Apply<MegaCrit.Sts2.Core.Models.Powers.StrengthPower>(Owner.Creature, tempStrength, Owner.Creature, this, false);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(5m);  // 15->20
    }
}
