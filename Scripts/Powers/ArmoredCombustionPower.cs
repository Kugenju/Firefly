using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;

namespace Firefly.Powers;

/// <summary>
/// 装甲燃烧：受到的伤害减少5点。
/// </summary>
[Pool(typeof(FireflyPowers))]
public class ArmoredCombustionPower : CustomPowerModel
{
    private const int DAMAGE_REDUCTION = 5;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override List<(string, string)> Localization => new PowerLoc(
        Title: "装甲燃烧",
        Description: "受到的伤害减少{0}点。",
        SmartDescription: "受到的伤害减少{0}点。"
    );

    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target != Owner || amount <= 0)
        {
            return 0m;
        }

        return -System.Math.Min(amount, DAMAGE_REDUCTION);
    }
}
