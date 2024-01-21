using System.Collections.Generic;
using Interfaces;
using Objects.Players;
using Objects.Players.Scripts;

namespace Objects.Characters.Alice
{
    public class AliceStrategy : CharacterStrategyBase, ICharacterStrategy
    {
        public void ApplyLevelUp(CharacterRank rank, int currentLevel, PlayerStatsComponent playerStatsComponent)
        {
            if (currentLevel % 10 == 0)
                playerStatsComponent.IncreaseCooldownReductionPercentage(0.015f);
            if (currentLevel % 10 == 0 && rank >= CharacterRank.E4 && PlayerStatsScaler.GetScaler().GetDodgeChance() < 0.4f)
                playerStatsComponent.IncreaseDodgeChance(0.02f);
        }

        public void ApplyRank(PlayerStats stats, CharacterRank characterRank)
        {
            if (characterRank >= CharacterRank.E5)
            {
                stats.SkillCooldownReductionPercentage += 0.3f;
            }
        }
    }
}