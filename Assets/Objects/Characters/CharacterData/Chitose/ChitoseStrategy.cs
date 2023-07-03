using Interfaces;
using Objects.Players;

namespace Objects.Characters.Chitose
{
	public class ChitoseStrategy : ICharacterStrategy
	{
		public void Apply(PlayerStats stats, CharacterRank characterRank)
		{
			if (characterRank >= CharacterRank.E1)
			{
				stats.MagnetSize += 0.1f;
			}

			if (characterRank >= CharacterRank.E2)
			{
				stats.DodgeChance += 0.1f;
			}

			if (characterRank >= CharacterRank.E4)
			{
				stats.DamagePercentageIncrease += 0.20f;
				stats.EnemySpawnRateIncreasePercentage += 0.05f;
			}

			if (characterRank >= CharacterRank.E5)
			{
				stats.DamageTakenIncreasePercentage += 0.15f; 
			}
		}
	}
}