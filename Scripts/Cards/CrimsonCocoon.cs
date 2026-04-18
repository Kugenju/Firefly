using BaseLib.Utils;
using Firefly.Scripts.CardPools;
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
/// 赤染之茧 - 普通技能牌
/// 获得5点格挡。生命值每低于最大值10点，格挡+2。升级：格挡+3。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class CrimsonCocoon : CardModel
{
    public CrimsonCocoon() : base(0, CardType.Skill, CardRarity.Common, TargetType.Self, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new BlockVar(5m, ValueProp.Move)
    };

    private const int BLOCK_BONUS_PER_10_HP = 2;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var ownerCreature = Owner?.Creature;
        if (ownerCreature == null) return;

        int baseBlock = (int)DynamicVars.Block.BaseValue;
        int lostHealth = ownerCreature.MaxHp - ownerCreature.CurrentHp;
        int bonusBlock = (lostHealth / 10) * BLOCK_BONUS_PER_10_HP;
        int finalBlock = baseBlock + bonusBlock;

        await CreatureCmd.GainBlock(ownerCreature, finalBlock, ValueProp.Move, cardPlay, false);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3m); // 5->8
    }
}
