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
		[SerializeField] private Image iconUpgrade;
		[SerializeField] private Image backgroundRarity;
		[SerializeField] private Image flashRarity;
		[SerializeField] private TextMeshProUGUI textRarity;
		[SerializeField] private ParticleSystem particlesRarity;
		[SerializeField] private Image backgroundElement;
		[SerializeField] private TextMeshProUGUI textElement;
		[SerializeField] private TextMeshProUGUI textUpgradeName;
		[SerializeField] private TextMeshProUGUI textUpgradeDescription;
		private UpgradePanelManager _upgradePanelManager;

		public void Set(UpgradeEntry upgradeEntry, UpgradePanelManager upgradePanelManager)
		{
			_upgradeEntry = upgradeEntry;
			_upgradePanelManager = upgradePanelManager;

			iconUpgrade.color = Color.white;
			iconUpgrade.sprite = _upgradeEntry.GetUnlockIcon();

			textUpgradeName.text = _upgradeEntry.GetUnlockName();
			textUpgradeDescription.text = _upgradeEntry.GetUnlockDescription();

			var p = particlesRarity.main;
			backgroundRarity.color = flashRarity.color = _upgradeEntry.GetUpgradeColor();
			p.startColor = new ParticleSystem.MinMaxGradient(_upgradeEntry.GetUpgradeColor());
			textRarity.text = _upgradeEntry.GetRarityName();

			var element = _upgradeEntry.GetElement();
			if (element != Element.Disabled)
			{
				backgroundElement.gameObject.SetActive(true);
				backgroundElement.color = ElementService.ElementToColor(element);
				textElement.text = element.ToString();
			}
			else
			{
				backgroundElement.gameObject.SetActive(false);
			}
		}

		public void Clean()
		{
			
		}
			

		public void SelectUpgrade()
		{
			_upgradePanelManager.Upgrade(_upgradeEntry);
		}
	}
}