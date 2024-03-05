using System;
using Data.Elements;
using Managers;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Labels.InGame.LevelUpScreen
{
	public class UpgradePanel : MonoBehaviour
	{ 
		private UpgradeEntry _upgradeEntry;
		[SerializeField] private Image icon;
		[SerializeField] private Image border;
		[SerializeField] private TextMeshProUGUI upgradeName;
		[SerializeField] private TextMeshProUGUI upgradeDescription;
		[SerializeField] private SVGImage elementIcon;
		[SerializeField] private ElementIconData elementIconData;
		[SerializeField] private Image upgradeBorder;
		private UpgradePanelManager _upgradePanelManager;

		public void Set(UpgradeEntry upgradeEntry, UpgradePanelManager upgradePanelManager)
		{
			_upgradeEntry = upgradeEntry;
			_upgradePanelManager = upgradePanelManager;

			icon.color = Color.white;
			icon.sprite = _upgradeEntry.GetUnlockIcon();

			upgradeName.text = _upgradeEntry.GetUnlockName();
			upgradeName.color = border.color = upgradeBorder.color = _upgradeEntry.GetUpgradeColor();
			upgradeDescription.text = _upgradeEntry.GetUnlockDescription();

			var element = _upgradeEntry.GetElement();
			if (element != Element.Disabled)
			{
				elementIcon.gameObject.SetActive(true);
				elementIcon.sprite = elementIconData.GetIcon(element);
				elementIcon.color = ElementService.ElementToColor(element);
			}
			else
				elementIcon.gameObject.SetActive(false);
		}

		public void Clean()
		{
			
		}
			

		public void SelectUpgrade()
		{
			_upgradeEntry.LevelUp(WeaponManager.instance);
			_upgradePanelManager.ClosePanel();
		}
	}
}