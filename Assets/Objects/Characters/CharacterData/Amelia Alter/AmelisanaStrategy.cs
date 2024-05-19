using Interfaces;
using Objects.Players;
using Objects.Players.Scripts;

namespace Objects.Characters.Amelia_Alter
{
    public class AmelisanaStrategy : CharacterStrategyBase, ICharacterStrategy
    {
        public void ApplyLevelUp(CharacterRank rank, int currentLevel, PlayerStatsComponent playerStatsComponent)
        {
            if (rank >= CharacterRank.E2)
                playerStatsComponent.IncreaseHealingReceived(0.005f);
        }

        public void ApplyRank(PlayerStats stats, CharacterRank characterRank)
        {
        }
    }
}