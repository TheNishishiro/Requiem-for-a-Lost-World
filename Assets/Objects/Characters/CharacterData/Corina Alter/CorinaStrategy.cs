using System.Collections.Generic;
using Interfaces;
using Objects.Players;
using Objects.Players.Scripts;

namespace Objects.Characters.Corina_Alter
{
	public class CorinaStrategy : ICharacterStrategy
	{
		public void ApplyRank(PlayerStats stats, CharacterRank characterRank)
		{
			if (characterRank >= CharacterRank.E1)
			{
				stats.MagnetSize += 0.2f;
				stats.DamageOverTime += stats.DamageOverTime * 0.3f;
			}
			if (characterRank >= CharacterRank.E3)
			{
				stats.CritDamage += 0.5f;
				stats.HealthMax -= 20;
			}
			if (characterRank >= CharacterRank.E4)
			{
				stats.Scale += 0.2f;
			}
			if (characterRank >= CharacterRank.E5)
			{
				stats.DamagePercentageIncrease += 0.5f;
				stats.MagnetSize += 0.2f;
			}
		}

		public void ApplySkillTree(PlayerStats stats, List<int> unlockedTreeNodeIds)
		{
		}

		public void ApplyLevelUp(CharacterRank rank, int currentLevel, PlayerStatsComponent playerStatsComponent)
		{
			if (currentLevel % 2 == 0 && rank >= CharacterRank.E2)
				playerStatsComponent.IncreaseHealingReceived(0.01f);
		}
	}
}