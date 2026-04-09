using BaseLib.Abstracts;
using System.Collections.Generic;
using Godot;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Unlocks;
using Firefly.Scripts.Relics;

namespace Firefly.Scripts.RelicPools;

/// <summary>
/// 流萤的遗物池
/// </summary>
public sealed class FireflyRelicPool : CustomRelicPoolModel
{
    // 使用默认能量图标
    public override string? TextEnergyIconPath => null;
    public override string? BigEnergyIconPath => null;

    // 实验室遗物轮廓颜色
    public override Color LabOutlineColor => new Color("E85D04");

    // 生成所有遗物
    protected override IEnumerable<RelicModel> GenerateAllRelics()
    {
        return new RelicModel[]
        {
            ModelDb.Relic<SamArmor>(),
            ModelDb.Relic<GransoursCore>(),
            ModelDb.Relic<StellaronFragment>(),
            ModelDb.Relic<IronCavalryInsignia>(),
            ModelDb.Relic<CombustionEngine>(),
            ModelDb.Relic<FireflyWings>(),
        };
    }

    // 获取已解锁的遗物
    public override IEnumerable<RelicModel> GetUnlockedRelics(UnlockState unlockState)
    {
        return base.AllRelics;
    }
}
