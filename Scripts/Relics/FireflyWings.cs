using BaseLib.Utils;
using Firefly.Scripts.RelicPools;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;

namespace Firefly.Scripts.Relics;

/// <summary>
/// 萤火之翼 - 角色专属普通遗物
/// </summary>
[Pool(typeof(FireflyRelicPool))]
public class FireflyWings : RelicModel
{
    public override RelicRarity Rarity => RelicRarity.Common;
}
