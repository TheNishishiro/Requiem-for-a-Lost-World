using System.Collections.Generic;
using Interfaces;
using Objects.Players;
using Objects.Players.Scripts;

namespace Objects.Characters.Maid
{
	public class ElizaStrategy : CharacterStrategyBase, ICharacterStrategy
	{
		public void ApplyRank(PlayerStats stats, CharacterRank characterRank)
		{
			if (characterRank >= CharacterRank.E4)
				stats.Speed += 0.25f;
		}

		public void ApplyLevelUp(CharacterRank rank, int currentLevel, PlayerStatsComponent playerStatsComponent)
		{
			var maxCritRate = rank >= CharacterRank.E3 ? 100 : 40;
			
			if (currentLevel <= maxCritRate && currentLevel % 10 == 0)
			{
				playerStatsComponent.IncreaseCritRate(0.05f);
			}

			if (rank >= CharacterRank.E5 && currentLevel % 5 == 0)
			{
				playerStatsComponent.IncreaseCritDamage(0.1f);
			}
		}
	}
}