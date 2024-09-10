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
		
		public ItemBase GetItemByName(string itemName)
		{
			return availableItems.FirstOrDefault(x => x.itemBase.NameField == itemName)?.itemBase;
		}

		public IPlayerItem GetItemByAchievement(AchievementEnum achievementEnum)
		{
			return availableItems.FirstOrDefault(w => w.itemBase.requiredAchievement == achievementEnum)?.itemBase;
		}
	}
}