using System.Collections.Generic;
using Interfaces;
using Objects.Players;
using Objects.Players.Scripts;

namespace Objects.Characters.Adam
{
	public class AdamStrategy : CharacterStrategyBase, ICharacterStrategy
	{
		public void ApplyLevelUp(CharacterRank rank, int currentLevel, PlayerStatsComponent playerStatsComponent)
		{
			if (rank >= CharacterRank.E3)
				playerStatsComponent.IncreaseLuck(0.05f);
		}

		public void ApplyRank(PlayerStats stats, CharacterRank characterRank)
		{
			if (characterRank >= CharacterRank.E4)
				stats.HealthRegen += 1;
		}
	}
}