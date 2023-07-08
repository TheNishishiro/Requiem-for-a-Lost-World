using Interfaces;
using Objects.Players;
using Objects.Players.Scripts;

namespace Objects.Characters.Chitose
{
	public class ChitoseStrategy : ICharacterStrategy
	{
		public void ApplyRank(PlayerStats stats, CharacterRank characterRank)
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

		public void ApplyLevelUp(CharacterRank rank, int currentLevel, PlayerStatsComponent playerStatsComponent)
		{
			var reduction = rank >= CharacterRank.E5 ? 5 : 0;
					
			if (currentLevel % (10 - reduction) == 0)
			{
				playerStatsComponent.IncreaseSpeed(0.05f);
			}

			if (rank >= CharacterRank.E3 && currentLevel % (20 - reduction) == 0)
			{
				playerStatsComponent.IncreaseCooldownReductionPercentage(0.08f);
			}
		}
	}
}