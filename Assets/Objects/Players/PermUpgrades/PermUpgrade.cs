using UnityEngine;

namespace Objects.Players.PermUpgrades
{
	[CreateAssetMenu]
	public class PermUpgrade : ScriptableObject
	{
		public PermUpgradeType type;
		public string name;
		public string description;
		public Sprite icon;
		public float basePrice;
		public float costPerLevel;
		public int maxLevel;
		public float increasePerLevel;
	}
}