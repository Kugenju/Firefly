using BaseLib.Utils;
using Firefly.Powers;
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
/// 赤金之梦 - 罕见技能牌
/// 移除一个敌人身上的所有灼热。每移除1层，回复1点生命值。升级：回复2点。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class GoldenDream : CardModel
{
    public GoldenDream() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => System.Array.Empty<DynamicVar>();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null || Owner?.Creature == null) return;

        // 获取目标身上的灼热层数
        var scorchPower = cardPlay.Target.Powers.FirstOrDefault(p => p is ScorchPower);
        if (scorchPower != null)
        {
            int scorchAmount = scorchPower.Amount;
            
            // 移除所有灼热
            await PowerCmd.Remove(scorchPower);

            // 回复生命
            int healPerScorch = IsUpgraded ? 2 : 1;
            int totalHeal = scorchAmount * healPerScorch;
            await CreatureCmd.Heal(Owner.Creature, totalHeal);
        }
    }

    protected override void OnUpgrade()
    {
        // 升级效果在 OnPlay 中处理
    }
}
