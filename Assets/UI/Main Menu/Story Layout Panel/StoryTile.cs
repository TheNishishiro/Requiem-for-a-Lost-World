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
		[SerializeField] private Sprite lockedSprite;
		[SerializeField] private Image uiImage;
		[SerializeField] private TextMeshProUGUI chapterNumberText;
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
		}

		public void Refresh()
		{
			if (imageSprite != null && isUnlocked)
				uiImage.sprite = imageSprite;
			else if (!isUnlocked)
				uiImage.sprite = lockedSprite;
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