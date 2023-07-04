using Interfaces;
using Objects.Players;

namespace Objects.Characters.David
{
	public class DavidStrategy : ICharacterStrategy
	{
		public void Apply(PlayerStats stats, CharacterRank characterRank)
		{
			if (characterRank >= CharacterRank.E1)
				stats.HealthRegen += 0.2f;
			if (characterRank >= CharacterRank.E2)
				stats.Armor += 1;
		}
	}
}