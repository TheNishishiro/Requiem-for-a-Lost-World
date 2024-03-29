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
		public int skillPoints;
		public List<int> unlockedSkillPoints;
		public Dictionary<StageEnum, int> FinishedDifficulty = new ();
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

		public int GetFinishedDifficulty(StageEnum stage)
		{
			FinishedDifficulty ??= new Dictionary<StageEnum, int>();
			FinishedDifficulty.TryAdd(stage, 0);
			return FinishedDifficulty[stage];
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

			if (gameResultData.IsWin && GetFinishedDifficulty(GameData.GetCurrentStage()) < (int)gameResultData.Difficulty + 1)
				FinishedDifficulty.TryAdd(GameData.GetCurrentStage(), (int)gameResultData.Difficulty + 1);
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