using System.Collections.Generic;
using Interfaces;
using Objects.Players;
using Objects.Players.Scripts;
using UnityEngine;

namespace Objects.Characters.Truzi
{
    public class TruziStrategy : CharacterStrategyBase, ICharacterStrategy
    {
        public void ApplyLevelUp(CharacterRank rank, int currentLevel, PlayerStatsComponent playerStatsComponent)
        {
            var luckValue = PlayerStatsScaler.GetScaler().GetLuck();
            if (Random.value < luckValue)
                playerStatsComponent.IncreaseCooldownReductionPercentage(0.05f);
        }

        public void ApplyRank(PlayerStats stats, CharacterRank characterRank)
        {
        }
    }
}