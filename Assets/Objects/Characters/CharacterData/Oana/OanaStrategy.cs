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
        }

        public void ApplyRank(PlayerStats stats, CharacterRank characterRank)
        {
            if (characterRank >= CharacterRank.E1)
            {
                stats.DamagePercentageIncrease += 0.75f;
            }
            if (characterRank >= CharacterRank.E4)
            {
                stats.SkillCooldownReductionPercentage += 0.25f;
                stats.CritRate += 0.3f;
                stats.CritDamage += 1.5f;
            }
        }
    }
}