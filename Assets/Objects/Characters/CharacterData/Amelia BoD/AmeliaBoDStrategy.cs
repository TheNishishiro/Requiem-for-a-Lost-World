using System.Collections.Generic;
using Interfaces;
using Objects.Players;
using Objects.Players.PermUpgrades;
using Objects.Players.Scripts;

namespace Objects.Characters.Amelia_BoD
{
	public class AmeliaBoDStrategy : CharacterStrategyBase, ICharacterStrategy
	{
		public void ApplyRank(PlayerStats stats, CharacterRank characterRank)
		{
		}

		public void ApplyLevelUp(CharacterRank rank, int currentLevel, PlayerStatsComponent playerStatsComponent)
		{
			if (currentLevel % 20 == 0)
			{
				playerStatsComponent.Add(StatEnum.EnemyHealthIncreasePercentage, 0.05f);
				playerStatsComponent.Add(StatEnum.EnemySpawnRateIncreasePercentage, 0.05f);
				if (currentLevel <= 80)
					playerStatsComponent.Add(StatEnum.AttackCount, 1);
			}
			
			if (currentLevel % 5 == 0 && rank >= CharacterRank.E5)
			{
				playerStatsComponent.Add(StatEnum.Damage, 1);
			}
		}
	}
}