using System;
using System.Collections.Generic;
using Objects.Characters;
using Objects.Stage;
using UnityEngine;

namespace DefaultNamespace.Data
{
	[Serializable]
	public class CharacterSaveData
	{
		public int Experience;
		public int Level;
		public bool IsUnlocked;
		public int RankUpLevel;
		public ulong KillCount;
		public int HighestInGameLevel;
		public bool FinishedGame;
		public int skillPoints;
		public List<int> unlockedSkillPoints;
		public int ExperienceNeeded => (int)(Level * 75 * 1.5f);

		public CharacterSaveData()
		{
			Level = 1;
		}

		public List<int> GetUnlockedSkillPoints()
		{
			unlockedSkillPoints ??= new List<int>();
			return unlockedSkillPoints;
		}

		public CharacterRank GetRankEnum()
		{
			return (CharacterRank)RankUpLevel;
		}
		
		public void Unlock()
		{
			if (!IsUnlocked)
				IsUnlocked = true;
			else if (RankUpLevel < (int)CharacterRank.E5)
				RankUpLevel++;
			else
				AddExperience(500);
		}
		
		public void AddGameResultStats(GameResultData gameResultData)
		{
			AddExperience(gameResultData.CharacterExp);
			KillCount += (ulong)gameResultData.MonstersKilled;
			if (gameResultData.Level > HighestInGameLevel)
				HighestInGameLevel = gameResultData.Level;

			if (!FinishedGame && gameResultData.IsWin)
				FinishedGame = true;
		}
		
		public void AddExperience(int experience)
		{
			Experience += experience;
			while (Experience >= ExperienceNeeded)
			{
				Experience -= ExperienceNeeded;
				Level++;
				skillPoints++;
			}
		}
	}
}