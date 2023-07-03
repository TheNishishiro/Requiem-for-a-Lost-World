using Objects.Characters;
using Objects.Players;

namespace Interfaces
{
	public interface ICharacterStrategy
	{
		void Apply(PlayerStats stats, CharacterRank characterRank);
	}
}