using BaseLib.Utils;
using Firefly.Scripts.CardPools;
using System.Collections.Generic;
using System.Linq;
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
/// 天台合影 - 稀有技能牌（目标：到天台合影）
/// 获得8点格挡。抽1张牌。如果手牌中有萤火牌，额外获得4点格挡并再抽1张牌。升级：获得11点格挡。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class RooftopPhoto : CardModel
{
    public RooftopPhoto() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new BlockVar(8m, ValueProp.Move)
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner?.Creature == null) return;

        // 检查手牌中是否有萤火牌
        bool hasFireflyCard = Owner.PlayerCombatState.Hand.Cards
            .Any(c => FireflyCardRegistry.IsFireflyCard(c) && c != this);

        int baseBlock = (int)DynamicVars.Block.BaseValue;
        int finalBlock = baseBlock;

        // 如果有萤火牌，额外获得格挡
        if (hasFireflyCard)
        {
            finalBlock += 4;
        }

        // 获得格挡
        await CreatureCmd.GainBlock(Owner.Creature, finalBlock, ValueProp.Move, cardPlay, false);

        // TODO: 抽牌（基础1张，有萤火牌额外1张）
        int drawCount = hasFireflyCard ? 2 : 1;
        await Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3m); // 8->11
    }
}
