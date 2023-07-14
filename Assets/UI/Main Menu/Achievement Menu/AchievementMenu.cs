using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using UI.Shared.Animations;
using UnityEngine;

namespace UI.Main_Menu.Achievement_Menu
{
	public class AchievementMenu : MonoBehaviour
	{
		[SerializeField] private SaveFile SaveFile;
		[SerializeField] private GameObject achievementEntryPrefab;
		[SerializeField] private GameObject achievementEntryContainer;
		[SerializeField] private SlideAnimator slideAnimator;
		private List<AchievementEntry> _achievementEntries;
		
		public void Open()
		{
			SaveFile = FindObjectOfType<SaveFile>();
			if (_achievementEntries == null)
			{
				_achievementEntries = new List<AchievementEntry>();
				foreach (var achievementEnum in Enum.GetValues(typeof(AchievementEnum)).Cast<AchievementEnum>())
				{
					var isCompleted = SaveFile.AchievementSaveData.TryGetValue(achievementEnum, out var value) && value;
					
					var entry = Instantiate(achievementEntryPrefab, achievementEntryContainer.transform).GetComponent<AchievementEntry>();
					entry.SetAchievement(achievementEnum, achievementEnum.GetDescription(), isCompleted);
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

			slideAnimator.ShowPanel();
		}
		
		public void Close()
		{
			slideAnimator.HidePanel();
		}

		public void Toggle()
		{
			if (gameObject.activeSelf)
				Close();
			else
				Open();
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
				Close();
		}
	}
}