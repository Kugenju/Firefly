using BaseLib.Utils;
using Firefly.Scripts.RelicPools;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;

namespace Firefly.Scripts.Relics;

/// <summary>
/// 星核碎片 - 角色专属稀有遗物
/// </summary>
[Pool(typeof(FireflyRelicPool))]
public class StellaronFragment : RelicModel
{
    public override RelicRarity Rarity => RelicRarity.Rare;
}
