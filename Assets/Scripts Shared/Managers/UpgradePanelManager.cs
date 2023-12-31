﻿using System.Collections.Generic;
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
		[SerializeField] private WeaponManager weaponManager;
		[SerializeField] private Button skipButton;
		[SerializeField] private Button rerollButton;
		[SerializeField] private PlayerStatsComponent playerStatsComponent;
		private bool _isWeaponOnly;
		
		private void Start()
		{
			HideButtons();
			if (GameData.GetPlayerCharacterData().PickWeaponOnStart)
				OpenPickWeapon();
		}

		public void OpenPanel()
		{
			_isWeaponOnly = false;
			WindowManager.instance.QueueWindow(this);
		}

		public void ClosePanel()
		{
			WindowManager.instance.DeQueueWindow();
		}

		private void OpenPickWeapon()
		{
			_isWeaponOnly = true;
			WindowManager.instance.QueueWindow(this);
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
			rerollButton.gameObject.SetActive(playerStatsComponent.HasRerolls());
			skipButton.gameObject.SetActive(playerStatsComponent.HasSkips());
			rerollButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Reroll ({playerStatsComponent.GetRerolls()})";
			skipButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Skip ({playerStatsComponent.GetSkips()})";
			var chanceOfAppearance = Random.value;
			var upgradesToPick = Random.Range(3, 5);
			
			var upgradeEntries = (_isWeaponOnly ? weaponManager.GetWeaponUnlocks() : weaponManager.GetUpgrades())
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
				upgradeButtons[i].Set(upgradeEntries[i]);
			}
		}
	}
}