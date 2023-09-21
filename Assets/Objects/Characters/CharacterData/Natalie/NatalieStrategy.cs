using Interfaces;
using Objects.Players;
using Objects.Players.Scripts;

namespace Objects.Characters.Natalie
{
	public class NatalieStrategy : ICharacterStrategy
	{
		public void ApplyLevelUp(CharacterRank rank, int currentLevel, PlayerStatsComponent playerStatsComponent)
		{
			if (currentLevel % 5 == 0 && rank >= CharacterRank.E4)
			{
				playerStatsComponent.IncreaseDamageOverTime(1);
			}
			if (currentLevel % 10 == 0 && rank >= CharacterRank.E4 && playerStatsComponent.GetMovementSpeed() < 4.0f)
			{
				playerStatsComponent.IncreaseMovementSpeed(playerStatsComponent.GetDamageOverTime() * 0.05f);
			}

		}

		public void ApplyRank(PlayerStats stats, CharacterRank characterRank)
		{
			if (characterRank >= CharacterRank.E1)
				stats.AttackCount += 1;
			if (characterRank >= CharacterRank.E2)
				stats.Speed += 1.5f;
			if (characterRank >= CharacterRank.E3)
			{
				stats.DamageOverTime += 5;
				stats.DamagePercentageIncrease += 0.1f;
			}
			if (characterRank >= CharacterRank.E5)
				stats.DamagePercentageIncrease += 0.3f;
		}
	}
}