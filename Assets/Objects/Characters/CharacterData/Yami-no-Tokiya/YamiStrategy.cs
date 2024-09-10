
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
            if (characterRank >= CharacterRank.E4)
            {
                stats.ResPen += 0.35f;
            }
        }

        public void ApplyLevelUp(CharacterRank rank, int currentLevel, PlayerStatsComponent playerStatsComponent)
        {
            
        }
    }
}