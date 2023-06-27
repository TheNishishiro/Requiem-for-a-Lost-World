using System;
using DefaultNamespace.Data;
using TMPro;
using UnityEngine;

namespace UI.Main_Menu
{
	public class GoldLabel : MonoBehaviour
	{
		private SaveFile _saveFile;
		private TextMeshProUGUI _text;

		public void Awake()
		{
			_saveFile = FindObjectOfType<SaveFile>();
			_text = GetComponent<TextMeshProUGUI>();
		}

		public void Update()
		{
			_text.text = $"{_saveFile.Gold}";
		}
	}
}