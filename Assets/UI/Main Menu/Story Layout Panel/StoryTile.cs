using System;
using TMPro;
using UI.Main_Menu.Lore_library;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_Menu.Story_Layout_Panel
{
	public class StoryTile : MonoBehaviour
	{
		public int chapterNumber;
		public int entryNumber;
		[SerializeField] private LoreEntry loreEntry;
		[SerializeField] private Sprite imageSprite;
		[SerializeField] private Image uiImage;
		[SerializeField] private TextMeshProUGUI chapterNumberText;

		private void Start()
		{
			if (imageSprite != null)
				uiImage.sprite = imageSprite;

			chapterNumberText.text = $"CH {chapterNumber}\n{entryNumber}";
		}
		
		public void OpenStoryEntry()
		{
			var storyLayoutPanel = FindObjectOfType<LoreEntryPanel>(true);
			storyLayoutPanel.gameObject.SetActive(true);
			storyLayoutPanel.Open(loreEntry);
		}
	}
}