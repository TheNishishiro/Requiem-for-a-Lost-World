using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Objects.Players;
using Objects.Players.Scripts;
using Objects.Stage;

namespace Objects.Characters.Amelia
{
	public class AmeliaStrategy : CharacterStrategyBase, ICharacterStrategy
	{
		public void ApplyLevelUp(CharacterRank rank, int currentLevel, PlayerStatsComponent playerStatsComponent)
		{
			if (currentLevel % 10 == 0 && rank < CharacterRank.E3 && currentLevel <= 40)
			{
				playerStatsComponent.IncreaseAttackCount(1);
			}
		}

		public void ApplyRank(PlayerStats stats, CharacterRank characterRank)
		{
			if (characterRank >= CharacterRank.E1)
				stats.HealingIncreasePercentage += 1f;
			if (characterRank >= CharacterRank.E2)
			{
				stats.DamagePercentageIncrease += 1f;
				stats.DamageTakenIncreasePercentage += 1f;
			}
			if (characterRank >= CharacterRank.E5)
			{
				stats.SpecialIncrease += 1;
			}
		}
	}
}