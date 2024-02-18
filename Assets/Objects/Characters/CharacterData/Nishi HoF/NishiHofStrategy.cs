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
                playerStatsComponent.IncreaseCritDamage(Random.Range(0.01f, 0.05f));
        }

        public void ApplyRank(PlayerStats stats, CharacterRank characterRank)
        {
            if (characterRank >= CharacterRank.E4)
                stats.DamagePercentageIncrease += 1f + 0.1f * GameData.GetPlayerCharacterSaveData().Level;
        }
    }
}