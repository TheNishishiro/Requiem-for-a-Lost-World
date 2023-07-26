using System;
using System.Collections.Generic;
using System.Linq;
using Objects.Characters;
using TMPro;
using UI.Main_Menu.Story_Layout_Panel;
using UnityEngine;

namespace UI.Main_Menu.Lore_library
{
	public class LoreEntryContainer : MonoBehaviour
	{
		[SerializeField] private GameObject authorPrefab;
		[SerializeField] private GameObject textBubblePrefab;
		private List<GameObject> _entries;

		public void Setup(LoreEntry loreEntry)
		{
			_entries ??= new List<GameObject>();
			SetupText(loreEntry);
		}

		private void SetupText(LoreEntry loreEntry)
		{
			var textBubbleGameObject = Instantiate(textBubblePrefab, transform);
			var textBubble = textBubbleGameObject.GetComponentInChildren<TextMeshProUGUI>();
			textBubble.text = loreEntry.TextFile.text;
			_entries.Add(textBubbleGameObject);
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