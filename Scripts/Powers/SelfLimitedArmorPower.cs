using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Firefly.Powers;

/// <summary>
/// 自限装甲能力 - 低生命时获得荆棘
/// </summary>
[Pool(typeof(FireflyPowers))]
public class SelfLimitedArmorPower : CustomPowerModel
{
    // 触发阈值
    private const int HEALTH_THRESHOLD = 20;
    private const int UPGRADED_HEALTH_THRESHOLD = 25;
    
    // 荆棘层数
    private const int THORNS_AMOUNT = 2;
    private const int UPGRADED_THORNS_AMOUNT = 3;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override List<(string, string)> Localization => new PowerLoc(
        Title: "自限装甲",
        Description: "生命值低于{0}点时，获得{1}层荆棘。",
        SmartDescription: "生命值低于{0}点时，获得{1}层荆棘。"
    );

    /// <summary>
    /// 回合开始时检查生命值
    /// </summary>
    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side != Owner.Side) return;

        // 检查当前生命
        if (Owner != null)
        {
            int threshold = Amount >= 2 ? UPGRADED_HEALTH_THRESHOLD : HEALTH_THRESHOLD;
            int thornsToGain = Amount >= 2 ? UPGRADED_THORNS_AMOUNT : THORNS_AMOUNT;

            // 如果生命值低于阈值，获得荆棘
            if (Owner.CurrentHp < threshold)
            {
                await PowerCmd.Apply<MegaCrit.Sts2.Core.Models.Powers.ThornsPower>(Owner, thornsToGain, Owner, null, false);
            }
        }
    }
}
