using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using System.Threading.Tasks;

namespace Firefly.Scripts.Enchantments;

/// <summary>
/// 激发附魔 - 给被激发的萤火卡牌添加金色发光效果
/// </summary>
[Pool(typeof(FireflyEnchantments))]
public class IgnitedEnchantment : EnchantmentModel
{
    /// <summary>
    /// 让卡牌显示金色发光效果
    /// </summary>
    public override bool ShouldGlowGold => true;

    /// <summary>
    /// 激发附魔不显示额外卡牌文本
    /// </summary>
    public override bool HasExtraCardText => false;

    /// <summary>
    /// 激发附魔不显示数量
    /// </summary>
    public override bool ShowAmount => false;

    /// <summary>
    /// 激发附魔可以栈叠（同一卡牌多次激发）
    /// </summary>
    public override bool IsStackable => true;

    /// <summary>
    /// 卡牌打出后自动清除激发附魔
    /// </summary>
    public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 卡牌打出时，让 FireflyIgnitionManager 清除激发状态
        if (Card != null)
        {
            FireflyIgnitionManager.ClearIgnition(Card);
        }
        await Task.CompletedTask;
    }
}
