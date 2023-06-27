using System;
using System.Collections.Generic;
using System.Linq;
using Objects.Characters;
using TMPro;
using UnityEngine;

namespace UI.Main_Menu.Lore_library
{
	public class LoreEntryContainer : MonoBehaviour
	{
		[SerializeField] private GameObject authorPrefab;
		[SerializeField] private GameObject textBubblePrefab;
		private List<GameObject> _entries;

		public void Setup(CharacterLoreEntry loreEntry)
		{
			_entries ??= new List<GameObject>();
			switch (loreEntry.Type)
			{
				case CharacterLoreEntry.LoreEntryType.Dialog:
					SetupDialog(loreEntry);
					break;
				case CharacterLoreEntry.LoreEntryType.Text:
					SetupText(loreEntry);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void SetupText(CharacterLoreEntry loreEntry)
		{
			var textBubbleGameObject = Instantiate(textBubblePrefab, transform);
			var textBubble = textBubbleGameObject.GetComponentInChildren<TextMeshProUGUI>();
			textBubble.text = loreEntry.TextFile.text;
			_entries.Add(textBubbleGameObject);
		}

		private void SetupDialog(CharacterLoreEntry loreEntry)
		{
			var dialogEntries = loreEntry.TextFile.text.Split('\n');
			foreach (var dialogEntry in dialogEntries)
			{
				if (dialogEntry.StartsWith("<character>"))
				{
					var authorGameObject = Instantiate(authorPrefab, transform);
					authorGameObject.GetComponent<TextMeshProUGUI>().text = dialogEntry.Substring(11);
					_entries.Add(authorGameObject);
					continue;
				}
				
				var textBubbleGameObject = Instantiate(textBubblePrefab, transform);
				var textBubble = textBubbleGameObject.GetComponentInChildren<TextMeshProUGUI>();
				textBubble.text = dialogEntry;
				_entries.Add(textBubbleGameObject);
			}
		}

		public void Clear()
		{
			_entries ??= new List<GameObject>();
			foreach (var entry in _entries)
			{
				Destroy(entry);
			}
		}
	}
}