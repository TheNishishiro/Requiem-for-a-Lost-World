using Interfaces;
using Objects.Players;
using Objects.Players.Scripts;

namespace Objects.Characters.Amelia_BoD
{
    public class AmelisanaStrategy : CharacterStrategyBase, ICharacterStrategy
    {
        public void ApplyLevelUp(CharacterRank rank, int currentLevel, PlayerStatsComponent playerStatsComponent)
        {
            if (rank >= CharacterRank.E3)
                playerStatsComponent.IncreaseHealingReceived(0.025f);
        }

        public void ApplyRank(PlayerStats stats, CharacterRank characterRank)
        {
            
        }
    }
}