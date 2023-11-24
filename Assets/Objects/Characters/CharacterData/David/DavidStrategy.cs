using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Objects.Players;
using Objects.Players.Scripts;
using Objects.Stage;
using UnityEngine;

namespace Objects.Characters.David
{
	public class DavidStrategy : CharacterStrategyBase, ICharacterStrategy
	{
		public void ApplyRank(PlayerStats stats, CharacterRank characterRank)
		{
		}

		public void ApplyLevelUp(CharacterRank rank, int currentLevel, PlayerStatsComponent playerStatsComponent)
		{
		}
	}
}