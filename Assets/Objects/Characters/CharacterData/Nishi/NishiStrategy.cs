using Interfaces;
using Objects.Players;
using Objects.Players.Scripts;

namespace Objects.Characters.Nishi
{
	public class NishiStrategy : ICharacterStrategy
	{
		public void ApplyLevelUp(CharacterRank rank, int currentLevel, PlayerStatsComponent playerStatsComponent)
		{
			
		}

		public void ApplyRank(PlayerStats stats, CharacterRank characterRank)
		{
			if (characterRank >= CharacterRank.E1)
			{
				stats.DodgeChance += 0.15f;
			}

			if (characterRank >= CharacterRank.E2)
			{
				stats.Luck += 0.06f;
				stats.SkillCooldownReductionPercentage += 0.15f;
			}
			
			if (characterRank >= CharacterRank.E3)
			{
				stats.HealthMax += 20;
			}
			
			if (characterRank >= CharacterRank.E4)
			{
				stats.HealthRegen += 0.1f;
			}
			
			if (characterRank >= CharacterRank.E5)
			{
				stats.SpecialMax -= 25;
			}
		}
	}
}