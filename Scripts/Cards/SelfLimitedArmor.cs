using BaseLib.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firefly.Powers;
using Firefly.Scripts.CardPools;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Firefly.Scripts.Cards;

/// <summary>
/// 自限装甲 - 普通能力牌
/// 
/// 稀有度：Common（普通）
/// 费用：2
/// 类型：Power（能力）
/// 目标：Self（自身）
/// 
/// 效果：生命值低于20点时，获得2层荆棘。
/// 升级：生命值低于25点时，获得3层荆棘。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class SelfLimitedArmor : CardModel
{
    // 触发阈值
    private const int HEALTH_THRESHOLD = 20;
    private const int UPGRADED_THRESHOLD = 25;
    
    // 荆棘层数
    private const int THORNS_AMOUNT = 2;
    private const int UPGRADED_THORNS = 3;

    public SelfLimitedArmor() 
        : base(2, CardType.Power, CardRarity.Common, TargetType.Self, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => System.Array.Empty<DynamicVar>();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner?.Creature == null) return;

        // 施加强化荆棘能力
        await PowerCmd.Apply<SelfLimitedArmorPower>(Owner.Creature, 1, Owner.Creature, this);

        await Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
        // 升级后费用不变，能力效果在Power中处理
    }
}
