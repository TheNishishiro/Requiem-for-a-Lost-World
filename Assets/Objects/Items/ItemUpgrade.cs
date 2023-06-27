using UnityEngine;

namespace Objects.Items
{
	[CreateAssetMenu]
	public class ItemUpgrade : ScriptableObject
	{
		public string Name;
		public string Description;
		public ItemStats ItemStats;
	}
}