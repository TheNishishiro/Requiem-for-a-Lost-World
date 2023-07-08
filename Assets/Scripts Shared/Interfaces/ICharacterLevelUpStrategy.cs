using Objects.Characters;
using Objects.Players.Scripts;

namespace Interfaces
{
	public interface ICharacterLevelUpStrategy
	{
		void ApplyLevelUp(CharacterRank rank, int currentLevel, PlayerStatsComponent playerStatsComponent);
	}
}