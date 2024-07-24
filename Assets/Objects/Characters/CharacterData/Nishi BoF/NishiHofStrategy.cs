using Interfaces;
using Objects.Players;
using Objects.Players.Scripts;
using Objects.Stage;
using UnityEngine;

namespace Objects.Characters.ishi_HoF
{
    public class NishiHofStrategy : CharacterStrategyBase, ICharacterStrategy
    {
        public void ApplyLevelUp(CharacterRank rank, int currentLevel, PlayerStatsComponent playerStatsComponent)
        {
            if (rank >= CharacterRank.E4)
                playerStatsComponent.IncreaseCritDamage(Random.Range(0.001f, 0.02f));
        }

        public void ApplyRank(PlayerStats stats, CharacterRank characterRank)
        {
            if (characterRank >= CharacterRank.E4)
                stats.DamagePercentageIncrease += 0.5f + 0.02f * GameData.GetPlayerCharacterSaveData().Level;
        }
    }
}