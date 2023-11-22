using System.Collections.Generic;
using Interfaces;
using Objects.Players;
using Objects.Players.Scripts;

namespace Objects.Characters.Chitose
{
	public class ChitoseStrategy : CharacterStrategyBase, ICharacterStrategy
	{
		public void ApplyRank(PlayerStats stats, CharacterRank characterRank)
		{
			if (characterRank >= CharacterRank.E2)
			{
				stats.DodgeChance += 0.25f;
			}

			if (characterRank >= CharacterRank.E4)
			{
				stats.DamagePercentageIncrease += 1f;
				stats.EnemySpawnRateIncreasePercentage += 0.2f;
			}

			if (characterRank >= CharacterRank.E5)
			{
				stats.SkillCooldownReductionPercentage += 0.25f;
			}
		}

		public void ApplyLevelUp(CharacterRank rank, int currentLevel, PlayerStatsComponent playerStatsComponent)
		{
			if (currentLevel % 10 == 0)
			{
				playerStatsComponent.IncreaseSpeed(0.05f);
			}
		}
	}
}