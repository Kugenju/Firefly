using BaseLib.Utils;
using Firefly.Scripts.RelicPools;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;

namespace Firefly.Scripts.Relics;

/// <summary>
/// 萨姆装甲 - 流萤的初始遗物
/// </summary>
[Pool(typeof(FireflyRelicPool))]
public class SamArmor : RelicModel
{
    public override RelicRarity Rarity => RelicRarity.Common;
}
