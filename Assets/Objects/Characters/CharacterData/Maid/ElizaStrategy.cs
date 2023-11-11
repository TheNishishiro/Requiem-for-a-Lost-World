using System.Collections.Generic;
using Interfaces;
using Objects.Players;
using Objects.Players.Scripts;

namespace Objects.Characters.Maid
{
	public class ElizaStrategy : ICharacterStrategy
	{
		public void ApplyRank(PlayerStats stats, CharacterRank characterRank)
		{
			if (characterRank >= CharacterRank.E1)
				stats.MovementSpeed += 0.1f;
			if (characterRank >= CharacterRank.E2)
				stats.DodgeChance += 0.08f;
			if (characterRank >= CharacterRank.E4)
				stats.Speed += 0.15f;
			if (characterRank >= CharacterRank.E5)
				stats.HealthMax -= 10;
		}

		public void ApplySkillTree(PlayerStats stats, List<int> unlockedTreeNodeIds)
		{
		}

		public void ApplyLevelUp(CharacterRank rank, int currentLevel, PlayerStatsComponent playerStatsComponent)
		{
			var maxCritRate = rank >= CharacterRank.E3 ? 70 : 40;
			
			if (currentLevel <= maxCritRate && currentLevel % 10 == 0)
			{
				playerStatsComponent.IncreaseCritRate(0.05f);
			}

			if (rank >= CharacterRank.E5 && currentLevel % 5 == 0)
			{
				playerStatsComponent.IncreaseCritDamage(0.03f);
			}
		}
	}
}