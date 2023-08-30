using System.Collections.Generic;
using System.Linq;
using Data.ToggleableEntries;
using UnityEngine;

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
	}
}