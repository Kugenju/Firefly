using BaseLib.Utils;
using Firefly.Scripts.RelicPools;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;

namespace Firefly.Scripts.Relics;

/// <summary>
/// 完全燃烧引擎 - 角色专属罕见遗物
/// </summary>
[Pool(typeof(FireflyRelicPool))]
public class CombustionEngine : RelicModel
{
    public override RelicRarity Rarity => RelicRarity.Uncommon;
}
