using Interfaces;
using Objects.Players;

namespace Objects.Characters.Arika
{
	public class ArikaStrategy : ICharacterStrategy
	{
		public void Apply(PlayerStats stats, CharacterRank characterRank)
		{
			if (characterRank >= CharacterRank.E1)
				stats.ProjectileLifeTimeIncreasePercentage += 0.3f;
			
			if (characterRank >= CharacterRank.E2)
			{
				stats.ProjectileLifeTimeIncreasePercentage += 0.2f;
				stats.ExperienceIncreasePercentage += 0.2f;
				stats.EnemySpawnRateIncreasePercentage += 0.07f;
			}

			if (characterRank >= CharacterRank.E4)
			{
				stats.Damage += 2f;
				stats.MagnetSize += 1f;
				stats.EnemyMaxCountIncreasePercentage += 0.15f;
			}

			if (characterRank >= CharacterRank.E5)
			{
				stats.SkillCooldownReductionPercentage += 0.25f;
				stats.EnemySpeedIncreasePercentage += 0.1f;
			}
		}
	}
}