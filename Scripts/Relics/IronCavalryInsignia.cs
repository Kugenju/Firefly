using BaseLib.Utils;
using Firefly.Scripts.RelicPools;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;

namespace Firefly.Scripts.Relics;

/// <summary>
/// 铁骑勋章 - 角色专属罕见遗物
/// </summary>
[Pool(typeof(FireflyRelicPool))]
public class IronCavalryInsignia : RelicModel
{
    public override RelicRarity Rarity => RelicRarity.Uncommon;
}
