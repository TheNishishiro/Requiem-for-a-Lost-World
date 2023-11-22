using System;
using System.Collections.Generic;
using Interfaces;
using Objects.Players;
using Objects.Players.Scripts;
using UnityEngine;

namespace Objects.Characters.Oana
{
    public class OanaStrategy : CharacterStrategyBase, ICharacterStrategy
    {
        public void ApplyLevelUp(CharacterRank rank, int currentLevel, PlayerStatsComponent playerStatsComponent)
        {
            if (rank >= CharacterRank.E3 && currentLevel % 10 == 0)
            {
                playerStatsComponent.IncreaseExperienceGain(0.05f);
            }
        }

        public void ApplyRank(PlayerStats stats, CharacterRank characterRank)
        {
            if (characterRank >= CharacterRank.E4)
            {
                stats.SkillCooldownReductionPercentage += 0.15f;
                stats.HealthRegen += 0.5f;
            }
        }
    }
}