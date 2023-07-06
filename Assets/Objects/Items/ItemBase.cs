using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
		public ICollection<StatsDisplayData> GetStatsData()
		{
			return ItemStats.GetDescription();
		}

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

		public void ApplyUpgrade(ItemUpgrade itemUpgrade, int rarity)
		{
			ItemStats.Apply(itemUpgrade.ItemStats, rarity);
		}

		public void ApplyRarity(int rarity)
		{
			ItemStats.ApplyRarity(rarity);
		}
	}
}