using System;
using Objects.Characters;
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

		public void Open(EidolonData eidolonData, bool isUnlocked)
		{
			titleText.text = eidolonData.EidolonName;
			descriptionText.text = eidolonData.EidolonDescription;
			if (isUnlocked)
				descriptionText.text += $"<br><br><br><color=#797979><size=80%><i>{eidolonData.EidolonQuote}</i></size></color>";
		}
	}
}