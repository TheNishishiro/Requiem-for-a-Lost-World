using NaughtyAttributes;
using UnityEngine;

namespace Objects.Abilities
{
	[CreateAssetMenu]
	public class UpgradeData : ScriptableObject
	{
		public string Name;
		[ResizableTextArea]
		public string Description;
		public WeaponStats WeaponStats;

		public string GetDescription(int rarity)
		{
			return WeaponStats.GetDescription(Description, rarity);
		}
	}
}