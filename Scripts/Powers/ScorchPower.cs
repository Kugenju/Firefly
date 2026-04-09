using BaseLib.Utils;
using Firefly.Scripts.CardPools;

namespace Firefly.Scripts.Powers;

/// <summary>
/// 灼热 - 核心机制
/// 带有灼热的敌人在显示攻击意图时，立即受到等于灼热层数的伤害，然后灼热层数减少1
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class ScorchPower
{
    public ScorchPower()
    {
    }
}
