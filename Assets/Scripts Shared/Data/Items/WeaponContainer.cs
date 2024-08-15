using System.Collections.Generic;
using System.Linq;
using Data.ToggleableEntries;
using DefaultNamespace.Data.Achievements;
using Interfaces;
using JetBrains.Annotations;
using Objects;
using UnityEngine;
using Weapons;

namespace DefaultNamespace.Data
{
	[CreateAssetMenu]
	public class WeaponContainer : BaseContainer
	{
		[SerializeField] private List<WeaponToggleableEntry> availableWeapons;
		private Dictionary<WeaponEnum, WeaponBase> _weaponDictionary;
		
		public List<WeaponToggleableEntry> GetWeapons()
		{
			return availableWeapons.ToList();
		}

		public WeaponBase GetWeapon(WeaponEnum weaponId)
		{
			_weaponDictionary ??= availableWeapons.ToDictionary(x => x.weaponBase.WeaponId, y => y.weaponBase);
			return _weaponDictionary[weaponId];
		}

		public IPlayerItem GetWeaponByAchievement(AchievementEnum achievementEnum)
		{
			return availableWeapons.FirstOrDefault(w => w.weaponBase.requiredAchievement == achievementEnum)?.weaponBase;
		}
	}
}