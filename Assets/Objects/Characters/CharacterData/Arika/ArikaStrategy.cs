using System.Collections.Generic;
using Interfaces;
using Objects.Players;
using Objects.Players.Scripts;

namespace Objects.Characters.Arika
{
	public class ArikaStrategy : CharacterStrategyBase, ICharacterStrategy
	{
		public void ApplyRank(PlayerStats stats, CharacterRank characterRank)
		{
			if (characterRank >= CharacterRank.E1)
				stats.ProjectileLifeTimeIncreasePercentage += 0.3f;
			
			if (characterRank >= CharacterRank.E2)
			{
				stats.ProjectileLifeTimeIncreasePercentage += 0.2f;
				stats.ExperienceIncreasePercentage += 0.2f;
				stats.EnemySpawnRateIncreasePercentage += 0.07f;
			}

			if (characterRank >= CharacterRank.E4)
			{
				stats.Damage += 2f;
				stats.MagnetSize += 1f;
				stats.EnemyMaxCountIncreasePercentage += 0.15f;
			}

			if (characterRank >= CharacterRank.E5)
			{
				stats.SkillCooldownReductionPercentage += 0.25f;
				stats.EnemySpeedIncreasePercentage += 0.1f;
			}
		}

		public void ApplyLevelUp(CharacterRank rank, int currentLevel, PlayerStatsComponent playerStatsComponent)
		{
			if (currentLevel % 20 == 0)
			{
				var amount = rank >= CharacterRank.E3 ? 0.07f : 0.1f;
				playerStatsComponent.IncreaseProjectileSize(amount);
			}
		}
	}
}