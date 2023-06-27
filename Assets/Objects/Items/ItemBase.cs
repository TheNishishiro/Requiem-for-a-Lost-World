using System;
using System.Collections.Generic;
using DefaultNamespace.Data;
using Interfaces;
using UnityEngine;

namespace Objects.Items
{
	public class ItemBase : MonoBehaviour, IPlayerItem
	{
		[SerializeField] public string Name;
		[SerializeField] public string Description;
		[SerializeField] public Sprite Icon;
		[SerializeField] public List<ItemUpgrade> ItemUpgrades;
		[SerializeField] public ItemStats ItemStats;

		public string NameField => Name;
		public string DescriptionField => Description;
		public Sprite IconField => Icon;
		public int LevelField { get; private set; } = 1;

		public IEnumerable<ItemUpgrade> GetAvailableUpgrades()
		{
			return ItemUpgrades;
		}

		public void RemoveUpgrade(ItemUpgrade itemUpgrade)
		{
			LevelField++;
			ItemUpgrades.Remove(itemUpgrade);
		}
		
		public virtual bool IsUnlocked(SaveFile saveFile)
		{
			return true;
		}
	}
}