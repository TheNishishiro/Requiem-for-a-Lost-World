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

			if (characterRank >= CharacterRank.E2)
				stats.SkillCooldownReductionPercentage += 0.20f;
			if (characterRank >= CharacterRank.E3)
			{
				stats.CritRate += 0.15f;
				stats.MovementSpeed += 0.05f;
			}
			if (characterRank >= CharacterRank.E4)
			{
				stats.EnemyMaxCountIncreasePercentage += 0.2f;
				stats.Luck += 0.15f;
			}
			if (characterRank >= CharacterRank.E5)
			{
				stats.DodgeChance += 0.2f;
				stats.ItemRewardIncrease += 0.15f;
				critDamageIncrease += 0.02f;
			}

			stats.CritDamage += GameData.GetPlayerCharacterSaveData().Level * critDamageIncrease;
			stats.Damage += (int)characterRank;
		}
	}
}