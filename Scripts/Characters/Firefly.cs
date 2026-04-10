using BaseLib.Abstracts;
using BaseLib.Utils;
using System.Collections.Generic;
using Godot;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.PotionPools;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Models.Relics;
using Firefly.Scripts.CardPools;
using Firefly.Scripts.Cards;
using Firefly.Scripts.PotionPools;
using Firefly.Scripts.RelicPools;
using Firefly.Scripts.Relics;

namespace Firefly.Scripts.Characters;

/// <summary>
/// 流萤角色 - 来自《崩坏：星穹铁道》的格拉默铁骑
/// 使用 PlaceholderCharacterModel 让缺失资源自动使用原版
/// </summary>
public sealed class Firefly : PlaceholderCharacterModel
{
    public const string CharacterId = "firefly";

    // ===== 基础属性（必须设置）=====
    public override Color NameColor => new Color("E85D04");
    public override Color EnergyLabelOutlineColor => new Color("D00000");
    public override CharacterGender Gender => CharacterGender.Feminine;
    public override int StartingHp => 70;
    public override int StartingGold => 99;

    // ===== 池子配置 =====
    public override CardPoolModel CardPool => ModelDb.CardPool<FireflyCardPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<FireflyPotionPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<FireflyRelicPool>();

    // ===== 初始牌组和遗物 =====
    // 初始卡组：打击x4, 防御x3, 熵增转移, 火萤斩击, 焦土陨击
    public override IEnumerable<CardModel> StartingDeck => new CardModel[]
    {
        ModelDb.Card<FireflyStrike>(),
        ModelDb.Card<FireflyStrike>(),
        ModelDb.Card<FireflyStrike>(),
        ModelDb.Card<FireflyStrike>(),
        ModelDb.Card<FireflyDefend>(),
        ModelDb.Card<FireflyDefend>(),
        ModelDb.Card<FireflyDefend>(),
        ModelDb.Card<EntropyTransfer>(), // 熵增转移 - 裂解机制核心卡
        ModelDb.Card<FlameLash>(),       // 火萤斩击 - 1费，施加灼热
        ModelDb.Card<MeteoricIncineration>(), // 焦土陨击 - 2费AOE，施加灼热
    };

    public override IReadOnlyList<RelicModel> StartingRelics => new[]
    {
        ModelDb.Relic<SamArmor>()
    };

    // ===== 角色选择界面资源 =====
    // 使用 Ironclad 资源作为占位符
    public override string CustomCharacterSelectIconPath => "res://images/packed/character_select/char_select_ironclad.png";
    public override string CustomCharacterSelectLockedIconPath => "res://images/packed/character_select/char_select_ironclad_locked.png";
    public override string CustomCharacterSelectBg => "res://scenes/screens/char_select/char_select_bg_ironclad.tscn";
    // public override string CustomCharacterSelectTransitionPath => "res://materials/transitions/ironclad_transition_mat.tres";

    // ===== 游戏内资源（使用原版占位）=====
    // public override string CustomIconPath => "res://images/ui/top_panel/character_icon_ironclad.png";
    // public override string CustomIconTexturePath => "res://images/ui/top_panel/character_icon_ironclad.png";
    // public override string CustomVisualPath => "res://scenes/creature_visuals/ironclad.tscn";
    // public override string CustomEnergyCounterPath => "res://scenes/combat/energy_counters/red_energy_counter.tscn";
    // public override string CustomTrailPath => "res://scenes/vfx/card_trail_red.tscn";
    // public override string CustomRestSiteAnimPath => "res://scenes/rest_site/characters/ironclad_rest_site.tscn";
    // public override string CustomMerchantAnimPath => "res://scenes/merchant/characters/ironclad_merchant.tscn";
    // public override string CustomMapMarkerPath => "res://images/packed/map/icons/map_marker_ironclad.png";

    // ===== 音效（使用原版）=====
    public override string CharacterTransitionSfx => "event:/sfx/ui/wipe_ironclad";

    // ===== 建筑师攻击特效 =====
    public override List<string> GetArchitectAttackVfx()
    {
        return new List<string>
        {
            "vfx/vfx_attack_slash",
            "vfx/vfx_fire_burst",
            "vfx/vfx_flame_impact"
        };
    }
}
