﻿using TMPro;
using UI.Labels.InGame.LevelUpScreen;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Labels.InGame.UpgradeScreen
{
	public class UpgradePanel : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI upgradeName;
		[SerializeField] private TextMeshProUGUI upgradeDescription;
		[SerializeField] private Image upgradeIcon;
		[SerializeField] private Image panelBackground;
		
		public void SetUpgradeData(UpgradeEntry upgradeEntry)
		{
			upgradeName.text = upgradeEntry.GetUnlockName();
			upgradeDescription.text = upgradeEntry.GetUnlockDescription();
			upgradeIcon.sprite = upgradeEntry.GetUnlockIcon();
			panelBackground.color = upgradeEntry.GetUpgradeColor();
		}

		public void Clean()
		{
			upgradeName.text = string.Empty;
			upgradeDescription.text = string.Empty;
			upgradeIcon.sprite = null;
		}
	}
}