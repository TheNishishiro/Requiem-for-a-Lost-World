using System;
using TMPro;
using UnityEngine;

namespace UI.Main_Menu.Character_List_Menu
{
	public class EidolonDescriptionPanel : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI titleText;
		[SerializeField] private TextMeshProUGUI descriptionText;

		private void Start()
		{
			titleText.text = string.Empty;
			descriptionText.text = string.Empty;
		}

		public void Open(string title, string description)
		{
			titleText.text = title;
			descriptionText.text = description;
		}
	}
}