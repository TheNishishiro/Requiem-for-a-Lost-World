using System;
using Objects.Characters;
using UI.Main_Menu.Story_Layout_Panel;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_Menu.Lore_library
{
	public class LoreEntryPanel : MonoBehaviour
	{
		[SerializeField] private LoreEntryContainer container;
		[SerializeField] private Image background;
		[SerializeField] private LineRenderer storyTileConnector;
		
		public void Open(LoreEntry loreEntry)
		{
			gameObject.SetActive(true);
			if (loreEntry.Background != null)
			{
				background.gameObject.SetActive(true);
				background.sprite = loreEntry.Background;
				background.color = Color.white;
			}
			else
			{
				background.gameObject.SetActive(false);
			}

			container.Setup(loreEntry);
		}
		
		public void Close()
		{
			gameObject.SetActive(false);
			container.Clear();
		}

		private void OnEnable()
		{
			storyTileConnector.enabled = false;
		}

		private void OnDisable()
		{
			storyTileConnector.enabled = true;
		}
	}
}