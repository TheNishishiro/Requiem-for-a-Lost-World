using System.Collections.Generic;
using System.Linq;
using Data.ToggleableEntries;
using DefaultNamespace.Data.Achievements;
using UnityEngine;

namespace DefaultNamespace.Data
{
	[CreateAssetMenu]
	public class ItemContainer : BaseContainer
	{
		[SerializeField] private List<ItemToggleableEntry> availableItems;
		
		public List<ItemToggleableEntry> GetItems()
		{
			return availableItems.ToList();
		}

		public Sprite GetIconByAchievement(AchievementEnum achievementEnum)
		{
			return availableItems.FirstOrDefault(w => w.itemBase.requiredAchievement == achievementEnum)?.itemBase?.Icon;
		}
	}
}