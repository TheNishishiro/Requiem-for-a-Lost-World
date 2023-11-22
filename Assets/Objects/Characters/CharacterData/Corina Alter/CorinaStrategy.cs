using System.Collections.Generic;
using Interfaces;
using Objects.Players;
using Objects.Players.Scripts;

namespace Objects.Characters.Corina_Alter
{
	public class CorinaStrategy : CharacterStrategyBase, ICharacterStrategy
	{
		public void ApplyRank(PlayerStats stats, CharacterRank characterRank)
		{
			if (characterRank >= CharacterRank.E2)
			{
				stats.DamagePercentageIncrease += 1.2f;
			}
			if (characterRank >= CharacterRank.E3)
			{
				stats.CritDamage += 1f;
			}
			if (characterRank >= CharacterRank.E5)
			{
				stats.SkillCooldownReductionPercentage += 0.2f;
			}
		}

		public void ApplyLevelUp(CharacterRank rank, int currentLevel, PlayerStatsComponent playerStatsComponent)
		{
			if (rank >= CharacterRank.E2)
				playerStatsComponent.IncreaseHealingReceived(0.01f);
		}
	}
}