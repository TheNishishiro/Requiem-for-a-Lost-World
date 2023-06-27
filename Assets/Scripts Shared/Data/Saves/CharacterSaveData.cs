using System;
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
		public int ExperienceNeeded => (int)(Level * 75 * 1.5f);

		public CharacterSaveData()
		{
			Level = 1;
		}

		public CharacterRank GetRankEnum()
		{
			return (CharacterRank)RankUpLevel;
		}
		
		public string GetRank()
		{
			var rank = "S";
			if (RankUpLevel >= (int)CharacterRank.SS)
				rank = "SS";
			if (RankUpLevel >= (int)CharacterRank.SSS)
				rank = "SSS";

			var rankDigit = RankUpLevel % 4;
			var displaySubDigit = rankDigit == 0 ? "" : rankDigit.ToString();
			return $"<cspace=-0.3em>{rank}</cspace><sub>{displaySubDigit}</sub>";
		}
		
		public void Unlock()
		{
			if (!IsUnlocked)
				IsUnlocked = true;
			else if (RankUpLevel <= 10)
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
		}
		
		public void AddExperience(int experience)
		{
			Experience += experience;
			while (Experience >= ExperienceNeeded)
			{
				Experience = 0;
				Level++;
			}
		}
	}
}