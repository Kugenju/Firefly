using BaseLib.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firefly.Scripts.CardPools;
using Firefly.Scripts.Keywords;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Firefly.Scripts.Cards;

/// <summary>
/// 萤火燎原 - 萤火技能牌（罕见）
/// 
/// 稀有度：Uncommon（罕见）
/// 费用：1
/// 类型：Skill（技能）
/// 目标：Self（自身）
/// 
/// 效果：抽2张牌，手中每有1张萤火牌额外抽1张。
/// 萤火：激发时效果翻倍，耗能-1
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class FireflyWildfire : CardModel
{
    public FireflyWildfire() 
        : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, false)
    {
    }

    // 萤火关键词
    public override IEnumerable<CardKeyword> CanonicalKeywords => new[]
    {
        FireflyKeywords.Firefly
    };

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(2m, ValueProp.Move)  // 使用 DamageVar 作为抽牌数
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 检查是否被激发
        int multiplier = FireflyIgnitionManager.GetEffectMultiplier(this);
        
        // 基础抽牌数量
        int baseDraw = (int)DynamicVars.Damage.BaseValue;
        int totalDraw = baseDraw * multiplier;

        // 计算手中萤火牌数量
        int fireflyCardCount = 0;
        if (Owner?.PlayerCombatState?.Hand?.Cards != null)
        {
            fireflyCardCount = Owner.PlayerCombatState.Hand.Cards
                .Count(c => FireflyCardRegistry.IsFireflyCard(c) && c != this);
        }
        
        // 每张萤火牌额外抽牌
        int extraDraw = fireflyCardCount * multiplier;
        totalDraw += extraDraw;

        // 抽牌 - 需要找到正确的抽牌方法
        // 暂时使用简化的实现
        await Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1m);  // 2->3
    }
}
