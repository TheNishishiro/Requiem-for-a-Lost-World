using UnityEngine;

namespace Objects.Abilities
{
	[CreateAssetMenu]
	public class UpgradeData : ScriptableObject
	{
		public string Name;
		public string Description;
		public WeaponStats WeaponStats;
	}
}