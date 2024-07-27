using Interfaces;
using Objects.Players;
using Objects.Players.Scripts;

namespace Objects.Characters.Yami_no_Tokiya
{
    public class YamiStrategy : CharacterStrategyBase, ICharacterStrategy
    {
        public void ApplyRank(PlayerStats stats, CharacterRank characterRank)
        {
        }

        public void ApplyLevelUp(CharacterRank rank, int currentLevel, PlayerStatsComponent playerStatsComponent)
        {
        }
    }
}