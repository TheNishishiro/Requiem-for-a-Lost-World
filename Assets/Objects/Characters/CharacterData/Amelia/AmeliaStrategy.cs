using Interfaces;
using Objects.Players;
using Objects.Players.Scripts;

namespace Objects.Characters.Amelia
{
	public class AmeliaStrategy : ICharacterStrategy
	{
		public void ApplyLevelUp(CharacterRank rank, int currentLevel, PlayerStatsComponent playerStatsComponent)
		{
			if (currentLevel % 30 == 0 && rank < CharacterRank.E3 && currentLevel <= 120)
			{
				playerStatsComponent.IncreaseAttackCount(1);
			}
		}

		public void ApplyRank(PlayerStats stats, CharacterRank characterRank)
		{
			if (characterRank >= CharacterRank.E1)
				stats.MovementSpeed += 0.1f;
			if (characterRank >= CharacterRank.E2)
			{
				stats.DamagePercentageIncrease += 0.2f;
				stats.DamageTakenIncreasePercentage += 0.2f;
			}
			if (characterRank >= CharacterRank.E5)
			{
				stats.SpecialIncrease += 1;
				stats.DamageTakenIncreasePercentage += 0.5f;
			}
		}
	}
}