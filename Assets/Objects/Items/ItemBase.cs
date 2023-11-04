using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Data.Elements;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Interfaces;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Objects.Items
{
	public class ItemBase : MonoBehaviour, IPlayerItem
	{
		[SerializeField] public string Name;
		[SerializeField] public string Description;
		[SerializeField] public float chanceToAppear;
		[SerializeField] public Sprite Icon;
		[SerializeField] public bool unlockOnAchievement;
		[ShowIf("unlockOnAchievement")]
		[SerializeField] public AchievementEnum requiredAchievement;
		[SerializeField] public List<ItemUpgrade> ItemUpgrades;
		[SerializeField] public ItemStats ItemStats;
		
		public string NameField => Name;
		public string DescriptionField => Description;
		public Sprite IconField => Icon;
		public int LevelField { get; private set; } = 1;
		public Element ElementField => Element.Disabled;

		public ICollection<StatsDisplayData> GetStatsData()
		{
			return ItemStats.GetStatsDisplayData();
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
			return !unlockOnAchievement || saveFile.IsAchievementUnlocked(requiredAchievement);
		}

		public bool ReliesOnAchievement(AchievementEnum achievement)
		{
			return unlockOnAchievement && achievement == requiredAchievement;
		}

		public void ApplyUpgrade(ItemUpgrade itemUpgrade, int rarity)
		{
			ItemStats.Apply(itemUpgrade.ItemStats, rarity);
		}

		public void ApplyRarity(int rarity)
		{
			ItemStats.ApplyRarity(rarity);
		}

		public string GetDescription(int rarity)
		{
			return ItemStats.GetDescription(Description, rarity);
		}
	}
}