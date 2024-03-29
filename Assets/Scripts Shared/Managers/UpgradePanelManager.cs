using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Interfaces;
using Objects.Players.Scripts;
using Objects.Stage;
using StarterAssets;
using TMPro;
using UI.Labels.InGame.LevelUpScreen;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Managers
{
	public class UpgradePanelManager : MonoBehaviour, IQueueableWindow
	{
		[SerializeField] private GameObject panel;
		[SerializeField] private List<UpgradePanel> upgradeButtons;
		[SerializeField] private Button skipButton;
		[SerializeField] private Button rerollButton;
		[SerializeField] private PlayerStatsComponent playerStatsComponent;
		private bool _isWeaponOnly;

		public void OpenPanel()
		{
			_isWeaponOnly = false;
			QueueableWindowManager.instance.QueueWindow(this);
		}

		public void ClosePanel()
		{
			QueueableWindowManager.instance.DeQueueWindow();
		}

		public void OpenPickWeapon()
		{
			_isWeaponOnly = true;
			QueueableWindowManager.instance.QueueWindow(this);
		}

		private void Clean()
		{
			foreach (var upgradeButton in upgradeButtons)
				upgradeButton.Clean();
		}

		private void HideButtons()
		{
			foreach (var upgradeButton in upgradeButtons)
				upgradeButton.gameObject.SetActive(false);
		}

		public void Reroll()
		{
			playerStatsComponent.IncreaseReroll(-1);
			ReloadUpgrades();
		}

		public void Skip()
		{
			playerStatsComponent.IncreaseSkip(-1);
			ClosePanel();
		}
		
		public void Open()
		{
			ReloadUpgrades();
		}

		public void Close()
		{
			HideButtons();
			panel.SetActive(false);
		}
		
		private void ReloadUpgrades()
		{
			HideButtons();
			rerollButton.gameObject.SetActive(PlayerStatsScaler.GetScaler().HasRerolls());
			skipButton.gameObject.SetActive(PlayerStatsScaler.GetScaler().HasSkips());
			rerollButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Reroll ({PlayerStatsScaler.GetScaler().GetRerolls()})";
			skipButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Skip ({PlayerStatsScaler.GetScaler().GetSkips()})";
			var chanceOfAppearance = Random.value;
			var upgradesToPick = Random.Range(3, 5);

			var upgradeEntries = (_isWeaponOnly ? WeaponManager.instance.GetWeaponUnlocks() : WeaponManager.instance.GetUpgrades())
				.OrderByDescending(x => x.ChanceOfAppearance >= 1 - chanceOfAppearance)
				.ThenBy(_ => Random.value)
				.Take(upgradesToPick)
				.ToList();
			if (upgradeEntries.Count == 0)
			{
				ClosePanel();
				return;
			}

			Clean();
			panel.SetActive(true);
			
			for (var i = 0; i < upgradeEntries.Count; i++)
			{
				upgradeButtons[i].gameObject.SetActive(true);
				upgradeButtons[i].Set(upgradeEntries[i], this);
			}
		}
	}
}