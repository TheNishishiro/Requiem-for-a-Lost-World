using Objects.Characters;
using Objects.Players;

namespace Interfaces
{
	public interface ICharacterStrategy : ICharacterSkillStrategy, ICharacterLevelUpStrategy
	{
		void ApplyRank(PlayerStats stats, CharacterRank characterRank);
	}
}