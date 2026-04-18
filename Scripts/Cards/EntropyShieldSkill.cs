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
/// 熵之盾 - 罕见技能牌
/// 获得等于当前失去生命值一半的格挡。升级：获得等于失去生命值的格挡。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class EntropyShieldSkill : CardModel
{
    public EntropyShieldSkill() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => System.Array.Empty<DynamicVar>();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var ownerCreature = Owner?.Creature;
        if (ownerCreature == null) return;

        int lostHealth = ownerCreature.MaxHp - ownerCreature.CurrentHp;
        int block = IsUpgraded ? lostHealth : lostHealth / 2;

        await CreatureCmd.GainBlock(ownerCreature, block, ValueProp.Move, cardPlay, false);
    }

    protected override void OnUpgrade()
    {
        // 升级效果在 OnPlay 中处理
    }
}
