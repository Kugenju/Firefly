using BaseLib.Utils;
using Firefly.Scripts.CardPools;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Firefly.Scripts.Cards;

/// <summary>
/// 冲锋 - 普通攻击牌
/// 造成8点伤害。如果本回合造成过伤害，伤害+4。升级：伤害+6。
/// </summary>
[Pool(typeof(FireflyCardPool))]
public class ChargeStrike : CardModel
{
    public ChargeStrike() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, false)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new DamageVar(8m, ValueProp.Move)
    };

    private const int BONUS_DAMAGE = 4;
    private const int UPGRADED_BONUS = 6;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null) return;

        int baseDamage = (int)DynamicVars.Damage.BaseValue;
        int finalDamage = baseDamage;
        var ownerCreature = Owner?.Creature;
        var combatState = ownerCreature?.CombatState;

        // 检查本回合是否造成过伤害
        if (combatState != null && ownerCreature != null)
        {
            bool dealtDamageThisTurn = CombatManager.Instance.History.Entries
                .OfType<DamageReceivedEntry>()
                .Any(entry =>
                    entry.HappenedThisTurn(combatState)
                    && entry.Dealer == ownerCreature
                    && entry.Result.TotalDamage > 0);

            if (dealtDamageThisTurn)
            {
                int bonus = IsUpgraded ? UPGRADED_BONUS : BONUS_DAMAGE;
                finalDamage += bonus;
            }
        }

        await DamageCmd.Attack(finalDamage)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m); // 8->10
    }
}
