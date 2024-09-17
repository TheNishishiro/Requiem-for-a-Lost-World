using System.Collections.Generic;
using System.Linq;
using Data.ToggleableEntries;
using DefaultNamespace.Data.Achievements;
using Interfaces;
using Objects.Items;
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
		
		public ItemBase GetItem(ItemEnum itemId)
		{
			return availableItems.FirstOrDefault(x => x.itemBase.ItemId == itemId)?.itemBase;
		}

		public IPlayerItem GetItemByAchievement(AchievementEnum achievementEnum)
		{
			return availableItems.FirstOrDefault(w => w.itemBase.requiredAchievement == achievementEnum)?.itemBase;
		}
	}
}