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
/// 燃命 - 罕见攻击牌
/// 失去等于当前能量值的生命值。造成等于失去生命值×7的伤害。升级：×10。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class LifeBurn : CardModel
{
    public LifeBurn() : base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => System.Array.Empty<DynamicVar>();

    private const int DAMAGE_MULTIPLIER = 7;
    private const int UPGRADED_MULTIPLIER = 10;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null || Owner?.Creature == null) return;

        // 获取当前能量值 - 简化实现
        // TODO: 找到正确的能量获取API
        int currentEnergy = 2; // 默认值，实际应该获取当前能量
        int multiplier = IsUpgraded ? UPGRADED_MULTIPLIER : DAMAGE_MULTIPLIER;
        int damage = currentEnergy * multiplier;

        // 失去生命值
        await CreatureCmd.Damage(
            choiceContext,
            Owner.Creature,
            currentEnergy,
            ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move,
            null,
            this
        );

        // 造成伤害
        await CreatureCmd.Damage(
            choiceContext,
            cardPlay.Target,
            damage,
            ValueProp.Move,
            Owner.Creature,
            this
        );
    }

    protected override void OnUpgrade()
    {
        // 升级效果在 OnPlay 中处理
    }
}
