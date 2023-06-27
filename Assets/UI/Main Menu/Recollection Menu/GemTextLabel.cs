using System;
using DefaultNamespace.Data;
using TMPro;
using UnityEngine;

namespace UI.Main_Menu.Recollection_Menu
{
	public class GemTextLabel : MonoBehaviour
	{
		private SaveFile _saveFile;
		[SerializeField] private TextMeshProUGUI gemText;

		private void Awake()
		{
			_saveFile = FindObjectOfType<SaveFile>();
		}

		public void Update()
		{
			if (Time.frameCount % 10 != 0)
				return;
			
			gemText.text = $"{_saveFile.Gems}";
		}
	}
}