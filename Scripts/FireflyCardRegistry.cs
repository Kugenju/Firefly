using System.Collections.Generic;
using System.Linq;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Commands;
using Firefly.Scripts.Enchantments;
using Firefly.Scripts.Keywords;

namespace Firefly.Scripts;

/// <summary>
/// 萤火卡牌注册表 - 用于标识哪些卡牌是萤火牌
/// 不使用 Keyword 系统，使用静态列表来标识
/// </summary>
public static class FireflyCardRegistry
{
    // 萤火卡牌 ID 列表
    private static readonly HashSet<string> _fireflyCardIds = new()
    {
        "FLASH_IGNITE_STRIKE",
        "ARMORED_DEFENSE",
        "FIREFLY_WILDFIRE",
        "IGNITE_THE_SEA",
        "FLAMES_SPREAD"
    };

    /// <summary>
    /// 检查卡牌是否是萤火牌
    /// </summary>
    public static bool IsFireflyCard(CardModel card)
    {
        return card.Keywords.Contains(FireflyKeywords.Firefly)
            || _fireflyCardIds.Contains(card.Id.Entry);
    }

    /// <summary>
    /// 注册新的萤火卡牌
    /// </summary>
    public static void RegisterFireflyCard(string cardId)
    {
        _fireflyCardIds.Add(cardId);
    }
}

/// <summary>
/// 萤火激发管理器 - 管理被激发的萤火牌
/// </summary>
public static class FireflyIgnitionManager
{
    // 存储被激发的卡片实例ID -> 卡牌引用（用于清除附魔）
    private static readonly Dictionary<ulong, CardModel> _ignitedCards = new();

    /// <summary>
    /// 激发一张卡片
    /// </summary>
    public static void IgniteCard(CardModel card)
    {
        var instanceId = (ulong)card.GetHashCode();
        
        if (!_ignitedCards.ContainsKey(instanceId))
        {
            // 存储卡牌引用
            _ignitedCards[instanceId] = card;
            
            // 减少能耗（最小为1）
            int currentCost = card.EnergyCost.GetResolved();
            int newCost = System.Math.Max(1, currentCost - 1);
            // 设置临时费用直到打出
            card.EnergyCost.SetUntilPlayed(newCost);
            card.InvokeEnergyCostChanged();
            
            // 应用激发附魔（金色发光效果）
            ApplyIgnitedEnchantment(card);
        }
    }

    /// <summary>
    /// 检查卡片是否被激发
    /// </summary>
    public static bool IsIgnited(CardModel card)
    {
        return _ignitedCards.ContainsKey((ulong)card.GetHashCode());
    }

    /// <summary>
    /// 获取效果倍数（激发时为2，否则为1）
    /// </summary>
    public static int GetEffectMultiplier(CardModel card)
    {
        return IsIgnited(card) ? 2 : 1;
    }

    /// <summary>
    /// 清除所有激发状态
    /// </summary>
    public static void ClearAllIgnitions()
    {
        // 清除所有卡牌的激发附魔
        foreach (var card in _ignitedCards.Values)
        {
            if (card != null && !card.HasBeenRemovedFromState)
            {
                ClearIgnitedEnchantment(card);
            }
        }
        _ignitedCards.Clear();
    }

    /// <summary>
    /// 清除单张卡牌的激发状态（在卡牌打出后调用）
    /// </summary>
    public static void ClearIgnition(CardModel card)
    {
        var instanceId = (ulong)card.GetHashCode();
        if (_ignitedCards.Remove(instanceId))
        {
            ClearIgnitedEnchantment(card);
        }
    }

    /// <summary>
    /// 应用激发附魔（金色发光）
    /// </summary>
    private static void ApplyIgnitedEnchantment(CardModel card)
    {
        // 如果卡牌已经有激发附魔，不需要重复应用
        if (card.Enchantment is IgnitedEnchantment)
        {
            return;
        }
        
        // 如果卡牌有其他附魔，先清除它（或者可以选择不覆盖）
        // 这里选择覆盖，因为激发效果优先级更高
        try
        {
            CardCmd.Enchant<IgnitedEnchantment>(card, 1);
        }
        catch
        {
            // 如果附魔失败（比如卡牌不能被附魔），忽略错误
        }
    }

    /// <summary>
    /// 清除激发附魔
    /// </summary>
    private static void ClearIgnitedEnchantment(CardModel card)
    {
        // 只清除激发附魔
        if (card.Enchantment is IgnitedEnchantment)
        {
            CardCmd.ClearEnchantment(card);
        }
    }
}
