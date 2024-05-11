using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Objects.Players;
using Objects.Players.Scripts;
using Objects.Stage;

namespace Objects.Characters.Summer
{
	public class SummerStrategy : CharacterStrategyBase, ICharacterStrategy
	{
		public void ApplyLevelUp(CharacterRank rank, int currentLevel, PlayerStatsComponent playerStatsComponent)
		{
		}

		public void ApplyRank(PlayerStats stats, CharacterRank characterRank)
		{
			var critDamageIncrease = 0.03f;

			if (characterRank >= CharacterRank.E1)
				stats.Damage += 2f * (int)characterRank;
			if (characterRank >= CharacterRank.E3)
			{
				stats.CritRate += 0.25f;
				stats.MovementSpeed += 0.15f;
			}
			if (characterRank >= CharacterRank.E4)
			{
				critDamageIncrease += 0.02f;
			}

			stats.CritDamage += GameData.GetPlayerCharacterSaveData().Level * critDamageIncrease;
			stats.Damage += (int)characterRank;
		}
	}
}