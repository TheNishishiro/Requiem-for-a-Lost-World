using Interfaces;
using Objects.Players;
using Objects.Players.Scripts;
using Objects.Stage;

namespace Objects.Characters.Summer
{
	public class SummerStrategy : ICharacterStrategy
	{
		public void ApplyLevelUp(CharacterRank rank, int currentLevel, PlayerStatsComponent playerStatsComponent)
		{
		}

		public void ApplyRank(PlayerStats stats, CharacterRank characterRank)
		{
			var critDamageIncrease = 0.03f;
			
			stats.CritDamage += GameData.GetPlayerCharacterSaveData().Level * critDamageIncrease;
		}
	}
}