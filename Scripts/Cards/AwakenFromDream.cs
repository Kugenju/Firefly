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
/// 从梦中醒来 - 罕见技能牌
/// 将目标敌人身上的所有格挡转移给自己。升级：转移的格挡+50%。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class AwakenFromDream : CardModel
{
    public AwakenFromDream() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => System.Array.Empty<DynamicVar>();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null || Owner?.Creature == null) return;

        int enemyBlock = cardPlay.Target.Block;
        if (enemyBlock > 0)
        {
            // 计算转移的格挡（升级+50%）
            int transferBlock = IsUpgraded ? (int)(enemyBlock * 1.5f) : enemyBlock;

            // 敌人失去格挡
            await CreatureCmd.LoseBlock(cardPlay.Target, enemyBlock);

            // 自己获得格挡
            await CreatureCmd.GainBlock(Owner.Creature, transferBlock, ValueProp.Move, cardPlay, false);
        }
    }

    protected override void OnUpgrade()
    {
        // 升级效果在 OnPlay 中处理
    }
}
