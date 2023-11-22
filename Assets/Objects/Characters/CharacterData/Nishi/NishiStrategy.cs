using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Objects.Players;
using Objects.Players.Scripts;
using Objects.Stage;

namespace Objects.Characters.Nishi
{
	public class NishiStrategy : CharacterStrategyBase, ICharacterStrategy
	{
		public void ApplyLevelUp(CharacterRank rank, int currentLevel, PlayerStatsComponent playerStatsComponent)
		{
			
		}

		public void ApplyRank(PlayerStats stats, CharacterRank characterRank)
		{
			if (characterRank >= CharacterRank.E1)
			{
				stats.DodgeChance += 0.2f;
			}
			
			if (characterRank >= CharacterRank.E3)
			{
				stats.DamagePercentageIncrease += 0.75f;
				stats.CritRate += 0.2f;
			}
		}
	}
}