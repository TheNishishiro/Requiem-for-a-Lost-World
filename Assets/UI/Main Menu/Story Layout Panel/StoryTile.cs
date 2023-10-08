using System;
using DefaultNamespace.Data;
using TMPro;
using UI.Main_Menu.Lore_library;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_Menu.Story_Layout_Panel
{
	public class StoryTile : MonoBehaviour
	{
		public ulong requiredStoryPoints;
		[SerializeField] public LoreEntry loreEntry;
		[SerializeField] private Sprite imageSprite;
		[SerializeField] private Image uiImage;
		[SerializeField] private TextMeshProUGUI chapterNumberText;
		[SerializeField] private TextMeshProUGUI progressionText;
		
		private SaveFile _saveFile;

		private SaveFile saveFile
		{
			get
			{
				if (_saveFile == null)
					_saveFile = FindObjectOfType<SaveFile>();
				return _saveFile;
			}
		}

		private bool isUnlocked => saveFile.StoryPoints >= requiredStoryPoints;

		private void Awake()
		{
			Refresh();
			chapterNumberText.text = $"Entry {loreEntry.EntryNumber}";
			uiImage.sprite = imageSprite;
		}

		public void Refresh()
		{
			progressionText.gameObject.SetActive(!isUnlocked);

			if (imageSprite != null && isUnlocked)
			{
				uiImage.color = Color.white;
			}
			else if (!isUnlocked)
			{
				progressionText.text = $"{(saveFile.StoryPoints/(float)requiredStoryPoints) * 100.0f:0}%";
				uiImage.color = Color.black;
			}
		}
	

		public void OpenStoryEntry()
		{
			if (!isUnlocked)
				return;
			
			var storyLayoutPanel = FindObjectOfType<LoreEntryPanel>(true);
			storyLayoutPanel.gameObject.SetActive(true);
			storyLayoutPanel.Open(loreEntry);
		}
	}
}