
using System;
using System.Linq;
using Interfaces;
using Objects.Players;
using Objects.Players.PermUpgrades;
using Objects.Players.Scripts;
using Random = UnityEngine.Random;

namespace Objects.Characters.Yami_no_Tokiya
{
    public class YamiStrategy : CharacterStrategyBase, ICharacterStrategy
    {
        public void ApplyRank(PlayerStats stats, CharacterRank characterRank)
        {
            if (characterRank >= CharacterRank.E2)
            {
                stats.CosmicDamageIncrease += 1f;
                stats.ElementalReactionEffectIncreasePercentage += 5f;
            }
        }

        public void ApplyLevelUp(CharacterRank rank, int currentLevel, PlayerStatsComponent playerStatsComponent)
        {
            if (rank >= CharacterRank.E4)
            {
                var statToUpgrade = Enum.GetValues(typeof(StatEnum)).Cast<StatEnum>().Except(new[]
                {
                    StatEnum.AttackCount, StatEnum.Revives, StatEnum.Rerolls, StatEnum.Skips, StatEnum.CooldownReduction
                }).Where(x => x.IsPercent()).OrderBy(_ => Random.value).FirstOrDefault();
                playerStatsComponent.Add(statToUpgrade, 0.01f);
            }
        }
    }
}