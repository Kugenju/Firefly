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
/// 永燃之心 - 稀有能力牌
/// 
/// 稀有度：Rare（稀有）
/// 费用：3
/// 类型：Power（能力）
/// 目标：Self（自身）
/// 
/// 效果：每回合开始时，对所有敌人施加2层灼热。
/// 升级：每回合施加3层灼热。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class EverburningHeart : CardModel
{
    // 灼热层数
    private const int SCORCH_AMOUNT = 2;
    private const int UPGRADED_SCORCH = 3;

    public EverburningHeart() 
        : base(3, CardType.Power, CardRarity.Rare, TargetType.Self, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => System.Array.Empty<DynamicVar>();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner?.Creature == null) return;

        // 施加永燃之心能力
        int scorchAmount = IsUpgraded ? UPGRADED_SCORCH : SCORCH_AMOUNT;
        await PowerCmd.Apply<EverburningHeartPower>(Owner.Creature, scorchAmount, Owner.Creature, this);

        await Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
        // 升级效果在施放时通过 IsUpgraded 判断
    }
}
