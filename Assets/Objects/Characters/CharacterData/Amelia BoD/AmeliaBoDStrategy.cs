using System.Collections.Generic;
using Interfaces;
using Objects.Players;
using Objects.Players.Scripts;

namespace Objects.Characters.Amelia_BoD
{
	public class AmeliaBoDStrategy : ICharacterStrategy
	{
		public void ApplyRank(PlayerStats stats, CharacterRank characterRank)
		{
			if (characterRank >= CharacterRank.E1)
			{
				stats.HealthMax += 20;
				stats.ProjectileLifeTimeIncreasePercentage += 0.4f;
			}
			if (characterRank >= CharacterRank.E2)
			{
				stats.HealingIncreasePercentage += 0.3f;
				stats.DamageTakenIncreasePercentage += 0.1f;
			}
			if (characterRank >= CharacterRank.E2)
			{
				stats.HealingIncreasePercentage += 0.3f;
				stats.DamageTakenIncreasePercentage += 0.1f;
			}
			if (characterRank >= CharacterRank.E5)
			{
				stats.EnemySpawnRateIncreasePercentage += 0.2f;
			}
		}

		public void ApplySkillTree(PlayerStats stats, List<int> unlockedTreeNodeIds)
		{
		}

		public void ApplyLevelUp(CharacterRank rank, int currentLevel, PlayerStatsComponent playerStatsComponent)
		{
			if (currentLevel <= 45 && currentLevel % 15 == 0)
			{
				playerStatsComponent.IncreaseAttackCount(1);
			}

			if (currentLevel % 15 == 0 && rank < CharacterRank.E3)
			{
				playerStatsComponent.IncreaseEnemyHealth(0.07f);
			}
			
			if (currentLevel % 13 == 0 && rank >= CharacterRank.E5)
			{
				playerStatsComponent.IncreaseFlatDamage(1);
			}
		}
	}
}