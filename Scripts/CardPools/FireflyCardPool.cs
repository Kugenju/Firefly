using BaseLib.Abstracts;
using System.Collections.Generic;
using System.Linq;
using Godot;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Unlocks;
using Firefly.Scripts.Cards;

namespace Firefly.Scripts.CardPools;

/// <summary>
/// 流萤的卡牌池
/// </summary>
public sealed class FireflyCardPool : CustomCardPoolModel
{
    // 卡池ID
    public override string Title => "firefly";

    // 使用默认能量图标（null表示使用原版）
    public override string? TextEnergyIconPath => null;
    public override string? BigEnergyIconPath => null;

    // 卡池主题色 - 火焰橙色
    public override Color DeckEntryCardColor => new Color("E85D04");

    // 卡框着色（使用默认卡框时有效）
    public override Color ShaderColor => new Color("E85D04");

    // 不是无色卡池
    public override bool IsColorless => false;

    // 生成所有卡牌
    protected override CardModel[] GenerateAllCards()
    {
        return new CardModel[]
        {
            ModelDb.Card<FireflyStrike>(),
            ModelDb.Card<FireflyDefend>(),
            ModelDb.Card<ChrysalidPyronexus>(),
            ModelDb.Card<MeteoricIncineration>(),
            ModelDb.Card<CompleteCombustionCard>(),
            ModelDb.Card<EmberBlade>(),
            ModelDb.Card<FlameLash>(),
            ModelDb.Card<HeatShield>(),
            ModelDb.Card<IgnitionDash>(),
            ModelDb.Card<SupernovaBurst>(),
            ModelDb.Card<PlasmaCage>(),
            ModelDb.Card<FlameDevour>(),
            ModelDb.Card<CombustionShield>(),
            ModelDb.Card<EntropyTransfer>(),
            ModelDb.Card<EntropyArmor>(),
            ModelDb.Card<StructuralCollapse>(),
            ModelDb.Card<DissolutionStrike>(),
            ModelDb.Card<EntropyReversal>(),
            ModelDb.Card<TotalCollapse>(),
            ModelDb.Card<MotherWrath>(),
        };
    }

    // 根据解锁状态过滤卡牌
    protected override IEnumerable<CardModel> FilterThroughEpochs(UnlockState unlockState, IEnumerable<CardModel> cards)
    {
        return cards.ToList();
    }
}
