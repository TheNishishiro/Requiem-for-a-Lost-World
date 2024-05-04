using System;
using System.Collections.Generic;
using Data.Difficulty;
using DefaultNamespace.Data;
using Managers;
using UnityEngine;
using Weapons;

namespace Objects.Stage
{
	public static class GameResultData
	{
		public static int Gold;
		public static int Gems;
		public static int MonstersKilled;
		public static Dictionary<WeaponBase, ulong> ItemDamage = new ();
		public static float Time;
		public static bool IsGameEnd;
		public static bool IsWin;
		public static int Level;
		public static DifficultyEnum Difficulty;
		public static int CharacterExp => (int) (MonstersKilled / 1000.0f + Time * 0.25f);

		public static void AddDamage(float damage, WeaponBase weaponBase)
		{
			if (weaponBase == null)
				return;

			if (weaponBase.isSkill)
				return;
			
			ItemDamage ??= new Dictionary<WeaponBase, ulong>();
			if (!ItemDamage.ContainsKey(weaponBase))
			{
				ItemDamage.Add(weaponBase, (ulong) damage);
				return;
			}

			ItemDamage[weaponBase] += (ulong) damage;
		}

		public static void Reset()
		{
			Gold = 0;
			Gems = 0;
			MonstersKilled = 0;
			ItemDamage = new Dictionary<WeaponBase, ulong>();
			Time = 0;
			IsGameEnd = false;
			IsWin = false;
		}
		
		public static List<string> GetStatsSummary()
		{
			var statsSummary = new List<string>();
			var minutes = (int) (Time / 60f);
			var seconds = (int) (Time % 60f);

			statsSummary.Add($"Time played: {minutes}:{seconds:00}");
			statsSummary.Add($"Character Exp: +{CharacterExp}");
			statsSummary.Add($"Kills: {MonstersKilled}");
			statsSummary.Add($"Gold: {Gold}");
			statsSummary.Add($"Gems: {Gems}");

			return statsSummary;
		}

		public static void AddGold(int goldAmount)
		{
			Gold += goldAmount;
		}

		public static void AddGems(int gemAmount)
		{
			Gems += gemAmount;
		}

		public static void FinalizeGameResult()
		{
			var currentDifficulty = GameData.GetCurrentDifficulty();
			Difficulty = currentDifficulty.Difficulty;
			
			var timeSpent = Time / 60.0f;
			var goldIncreaseByTime = timeSpent * 0.03f * currentDifficulty.RewardModifier;
			var gemIncreaseByTime = timeSpent * 0.05f * currentDifficulty.RewardModifier;
							
			Gold += (int)((Gold * (1 + goldIncreaseByTime) + MonstersKilled / 35.0f) * currentDifficulty.RewardModifier);
			Gems += (int)(MonstersKilled / 100.0f * currentDifficulty.RewardModifier * (1 + gemIncreaseByTime));
			if (IsWin)
			{
				Gold += 2000;
				Gems += 600;
			}

			IsGameEnd = false;
		}
	}
}