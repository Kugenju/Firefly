using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Firefly.Powers;

/// <summary>
/// 过载核心能力 - 每当你失去生命值，获得力量
/// 
/// 机制完全同战士的"撕裂"：失去生命时立即获得力量
/// </summary>
[Pool(typeof(FireflyPowers))]
public class OverloadCorePower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override List<(string, string)> Localization => new PowerLoc(
        Title: "过载核心",
        Description: "每当你失去生命值，获得{0}层力量。",
        SmartDescription: "每当你失去生命值，获得{0}层力量。"
    );

    /// <summary>
    /// 受到伤害后触发：如果实际失去了生命值，获得力量
    /// </summary>
    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature dealer, CardModel? cardSource)
    {
        // 只对自己生效
        if (target != Owner) return;

        // 如果实际失去了生命值（有未被格挡的伤害）
        if (result.UnblockedDamage > 0)
        {
            // 获得力量（层数等于Power的Amount）
            if (Owner != null)
            {
                await PowerCmd.Apply<MegaCrit.Sts2.Core.Models.Powers.StrengthPower>(Owner, Amount, Owner, null, false);
            }
        }
    }
}
