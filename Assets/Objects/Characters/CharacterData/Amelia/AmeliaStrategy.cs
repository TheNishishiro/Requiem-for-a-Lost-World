using Interfaces;
using Objects.Players;

namespace Objects.Characters.Amelia
{
	public class AmeliaStrategy : ICharacterStrategy
	{
		public void Apply(PlayerStats stats, CharacterRank characterRank)
		{
			if (characterRank >= CharacterRank.E1)
			{
				stats.HealthMax += 20;
				stats.ProjectileLifeTimeIncreasePercentage += 0.4f;
			}
			if (characterRank >= CharacterRank.E2)
			{
				stats.HealingIncreasePercentage += 0.3f;
				stats.DamageTakenIncreasePercentage += 0.1f;
			}
			if (characterRank >= CharacterRank.E2)
			{
				stats.HealingIncreasePercentage += 0.3f;
				stats.DamageTakenIncreasePercentage += 0.1f;
			}
			if (characterRank >= CharacterRank.E5)
			{
				stats.EnemySpawnRateIncreasePercentage += 0.2f;
			}
		}
	}
}