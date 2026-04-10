using System.Collections.Generic;
using System.Linq;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Cards;

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
        return _fireflyCardIds.Contains(card.Id.Entry);
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
    // 存储被激发的卡片实例ID
    private static readonly HashSet<ulong> _ignitedCardIds = new();

    /// <summary>
    /// 激发一张卡片
    /// </summary>
    public static void IgniteCard(CardModel card)
    {
        var instanceId = (ulong)card.GetHashCode();
        
        if (_ignitedCardIds.Add(instanceId))
        {
            // 减少能耗（最小为1）
            int currentCost = card.EnergyCost.GetResolved();
            int newCost = System.Math.Max(1, currentCost - 1);
            // 设置临时费用直到打出
            card.EnergyCost.SetUntilPlayed(newCost);
            card.InvokeEnergyCostChanged();
        }
    }

    /// <summary>
    /// 检查卡片是否被激发
    /// </summary>
    public static bool IsIgnited(CardModel card)
    {
        return _ignitedCardIds.Contains((ulong)card.GetHashCode());
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
        _ignitedCardIds.Clear();
    }
}
