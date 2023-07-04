using Interfaces;
using Objects.Players;

namespace Objects.Characters.Maid
{
	public class ElizaStrategy : ICharacterStrategy
	{
		public void Apply(PlayerStats stats, CharacterRank characterRank)
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
	}
}