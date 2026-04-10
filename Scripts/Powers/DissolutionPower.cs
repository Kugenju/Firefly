using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace Firefly.Powers;

/// <summary>
/// 裂解 (Dissolution) - 标记Power
/// 
/// 表示该敌人已被裂解，用于卡牌效果判断（如裂解打击）。
/// 这是一个视觉标记，实际裂解逻辑由DissolutionSourcePower处理。
/// </summary>
public class DissolutionPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override List<(string, string)> Localization => new PowerLoc(
        Title: "裂解",
        Description: "格挡被击破时会受到反馈伤害。",
        SmartDescription: "格挡被击破时会受到反馈伤害。"
    );
}
