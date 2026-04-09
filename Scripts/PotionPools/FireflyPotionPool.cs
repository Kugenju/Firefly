using BaseLib.Abstracts;
using System.Collections.Generic;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Potions;

namespace Firefly.Scripts.PotionPools;

/// <summary>
/// 流萤的药水池 - 使用共享药水池
/// </summary>
public sealed class FireflyPotionPool : CustomPotionPoolModel
{
    // 使用默认能量图标
    public override string? TextEnergyIconPath => null;
    public override string? BigEnergyIconPath => null;

    // 流萤使用标准药水池，不生成专属药水
    protected override IEnumerable<PotionModel> GenerateAllPotions()
    {
        return System.Array.Empty<PotionModel>();
    }
}
