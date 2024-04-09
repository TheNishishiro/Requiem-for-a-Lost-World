using System.Collections.Generic;
using System.Linq;
using Data.ToggleableEntries;
using DefaultNamespace.Data.Achievements;
using Interfaces;
using JetBrains.Annotations;
using UnityEngine;
using Weapons;

namespace DefaultNamespace.Data
{
	[CreateAssetMenu]
	public class WeaponContainer : BaseContainer
	{
		[SerializeField] private List<WeaponToggleableEntry> availableWeapons;
		
		public List<WeaponToggleableEntry> GetWeapons()
		{
			return availableWeapons.ToList();
		}

		public IPlayerItem GetWeaponByAchievement(AchievementEnum achievementEnum)
		{
			return availableWeapons.FirstOrDefault(w => w.weaponBase.requiredAchievement == achievementEnum)?.weaponBase;
		}
	}
}