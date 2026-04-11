using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace Firefly.Scripts.Keywords;

/// <summary>
/// 萤火关键词定义
/// </summary>
public class FireflyKeywords
{
    /// <summary>
    /// 萤火关键词
    /// 使用 CustomEnum 添加自定义关键词
    /// </summary>
    [CustomEnum("FIREFLY")]
    [KeywordProperties(AutoKeywordPosition.Before)]
    public static CardKeyword Firefly;
}
