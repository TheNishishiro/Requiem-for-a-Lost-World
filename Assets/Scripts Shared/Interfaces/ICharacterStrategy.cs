using System.Collections.Generic;
using Objects.Characters;
using Objects.Players;

namespace Interfaces
{
	public interface ICharacterStrategy : ICharacterSkillStrategy, ICharacterLevelUpStrategy
	{
		void ApplyRank(PlayerStats stats, CharacterRank characterRank);
		void ApplySkillTree(PlayerStats stats, List<int> unlockedTreeNodeIds);
	}
}