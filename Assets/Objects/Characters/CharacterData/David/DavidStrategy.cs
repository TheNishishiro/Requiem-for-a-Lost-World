using System;
using System.Collections;
using Interfaces;
using Objects.Players;
using Objects.Players.Scripts;
using Objects.Stage;
using UnityEngine;

namespace Objects.Characters.David
{
	public class DavidStrategy : ICharacterStrategy
	{
		public void ApplyRank(PlayerStats stats, CharacterRank characterRank)
		{
			if (characterRank >= CharacterRank.E1)
				stats.HealthRegen += 0.2f;
			if (characterRank >= CharacterRank.E2)
				stats.Armor += 1;
		}

		public void ApplyLevelUp(CharacterRank rank, int currentLevel, PlayerStatsComponent playerStatsComponent)
		{
			if (currentLevel % 12 == 0 && rank >= CharacterRank.E3)
			{
				playerStatsComponent.IncreaseDamageIncreasePercentage(0.05f);
			}
		}
	}
}