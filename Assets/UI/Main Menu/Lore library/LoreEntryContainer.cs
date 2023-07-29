using System;
using System.Collections;
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
		private Coroutine _printTextCoroutine;
		private TextMeshProUGUI _textField;
		private string _fullText;

		public void Setup(LoreEntry loreEntry)
		{
			_entries ??= new List<GameObject>();
			SetupText(loreEntry);
		}

		private void SetupText(LoreEntry loreEntry)
		{
			var textBubbleGameObject = Instantiate(textBubblePrefab, transform);
			_textField = textBubbleGameObject.GetComponentInChildren<TextMeshProUGUI>();
			_fullText = loreEntry.TextFile.text;
			_printTextCoroutine = StartCoroutine(PrintTextOverTime(_textField, _fullText, 0.04f));
			_entries.Add(textBubbleGameObject);
		}

		private void Update()
		{
			if (Input.GetMouseButtonUp(0) && _printTextCoroutine != null)
			{
				StopCoroutine(_printTextCoroutine);
				_textField.text = _fullText;
			}
		}

		private IEnumerator PrintTextOverTime(TMP_Text textField, string fullText, float delay)
		{
			textField.text = string.Empty;
			foreach (var letter in fullText)
			{
				textField.text += letter;
				yield return new WaitForSeconds(delay);
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