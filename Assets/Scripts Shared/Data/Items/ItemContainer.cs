using System.Collections.Generic;
using System.Linq;
using Data.ToggleableEntries;
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
	}
}