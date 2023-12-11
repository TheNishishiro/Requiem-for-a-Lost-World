using System;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

namespace Objects.Stage
{
	[CreateAssetMenu]
	public class GameResultData : ScriptableObject
	{
		public int Gold;
		public int Gems;
		public int MonstersKilled;
		public Dictionary<WeaponBase, ulong> ItemDamage = new ();
		public float Time;
		public bool IsGameEnd;
		public bool IsWin;
		public int Level;
		public int CharacterExp => (int) (MonstersKilled / 1000.0f + Time * 0.25f);

		public void AddDamage(float damage, WeaponBase weaponBase)
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

		public void Reset()
		{
			Gold = 0;
			Gems = 0;
			MonstersKilled = 0;
			ItemDamage = new Dictionary<WeaponBase, ulong>();
			Time = 0;
			IsGameEnd = false;
			IsWin = false;
		}
		
		public List<string> GetStatsSummary()
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

		public void AddGold(int goldAmount)
		{
			Gold += goldAmount;
		}

		public void AddGems(int gemAmount)
		{
			Gems += gemAmount;
		}

		public void FinalizeGameResult()
		{
			var timeSpent = Time / 60.0f;
			var goldIncreaseByTime = timeSpent * 0.03f;
			var gemIncreaseByTime = timeSpent * 0.05f;
							
			Gold += (int)(Gold * (1 + goldIncreaseByTime) + MonstersKilled / 10.0f);
			Gems += (int)(MonstersKilled / 100.0f * (1 + gemIncreaseByTime));
			if (IsWin)
			{
				Gold += 2000;
				Gems += 750;
			}
		}
	}
}