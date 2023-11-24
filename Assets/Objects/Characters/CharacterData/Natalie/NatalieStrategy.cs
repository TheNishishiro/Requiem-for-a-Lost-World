using System.Collections.Generic;
using Interfaces;
using Objects.Players;
using Objects.Players.Scripts;

namespace Objects.Characters.Natalie
{
	public class NatalieStrategy : CharacterStrategyBase, ICharacterStrategy
	{
		public void ApplyLevelUp(CharacterRank rank, int currentLevel, PlayerStatsComponent playerStatsComponent)
		{
			if (currentLevel % 5 == 0 && rank >= CharacterRank.E2)
			{
				playerStatsComponent.IncreaseDamageOverTime(2);
			}
		}

		public void ApplyRank(PlayerStats stats, CharacterRank characterRank)
		{
			if (characterRank >= CharacterRank.E4)
			{
				stats.DamageOverTimeDurationIncreasePercentage += 0.5f;
				stats.DamageOverTimeFrequencyReductionPercentage += 0.5f;
			}
		}
	}
}