using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Interfaces;
using NUnit.Framework;
using UI.Shared.Animations;
using UnityEngine;

namespace UI.Main_Menu.Achievement_Menu
{
	public class AchievementMenu : MonoBehaviour
	{
		[SerializeField] private SaveFile SaveFile;
		[SerializeField] private GameObject achievementEntryPrefab;
		[SerializeField] private GameObject achievementEntryContainer;
		[SerializeField] private WeaponContainer weaponContainer;
		[SerializeField] private ItemContainer itemContainer;
		private List<AchievementEntry> _achievementEntries;
		
		public void Open()
		{
			SaveFile = FindObjectOfType<SaveFile>();
			var items = new List<IPlayerItem>();
			items.AddRange(weaponContainer.GetWeapons().Select(x => x.weaponBase));
			items.AddRange(itemContainer.GetItems().Select(x => x.itemBase));
			
			if (_achievementEntries == null)
			{
				_achievementEntries = new List<AchievementEntry>();
				foreach (var achievementEnum in Enum.GetValues(typeof(AchievementEnum)).Cast<AchievementEnum>())
				{
					var isCompleted = SaveFile.AchievementSaveData.TryGetValue(achievementEnum, out var value) && value;
					var itemIcon = items.FirstOrDefault(x => x.ReliesOnAchievement(achievementEnum))?.IconField;
					
					var entry = Instantiate(achievementEntryPrefab, achievementEntryContainer.transform).GetComponent<AchievementEntry>();
					entry.SetAchievement(achievementEnum, achievementEnum.GetTitle(), achievementEnum.GetDescription(), isCompleted, itemIcon);
					_achievementEntries.Add(entry);
				}
			}
			else
			{
				foreach (var achievementEntry in _achievementEntries)
				{
					if (SaveFile.AchievementSaveData.TryGetValue(achievementEntry.Achievement, out var isCompleted) && isCompleted)
					{
						achievementEntry.MarkUnlocked();
					}
				}
			}
			
			gameObject.SetActive(true);
		}
		
		public void Close()
		{
			gameObject.SetActive(false);
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
				Close();
		}
	}
}