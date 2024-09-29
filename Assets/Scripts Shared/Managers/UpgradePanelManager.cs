using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Weapons;
using Interfaces;
using Objects.Players.Scripts;
using Objects.Stage;
using StarterAssets;
using TMPro;
using UI.Labels.InGame.LevelUpScreen;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
		private bool _waitingForPollResult;
		private float _timeOpened;

		private void Update()
		{
			_timeOpened += Time.unscaledDeltaTime;
		}

		public void OpenPanel()
		{
			_isWeaponOnly = false;
			QueueableWindowManager.instance.QueueWindow(this, false);
		}

		public void ClosePanel()
		{
			QueueableWindowManager.instance.DeQueueWindow(false);
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
			if (_waitingForPollResult) return;
			
			playerStatsComponent.IncreaseReroll(-1);
			ReloadUpgrades();
		}

		public void Skip()
		{
			if (_waitingForPollResult) return;
			
			playerStatsComponent.IncreaseSkip(-1);
			ClosePanel();
		}
		
		public void Open()
		{
			_timeOpened = 0;
			ReloadUpgrades();
			PauseManager.instance.PauseGame(true);
		}

		public void Close()
		{
			HideButtons();
			panel.SetActive(false);
			PauseManager.instance.UnPauseGame(true);
			GameManager.instance.playerMpComponent.VoteUnpause();
		}
		
		private void ReloadUpgrades()
		{
			HideButtons();
			rerollButton.gameObject.SetActive(PlayerStatsScaler.GetScaler().HasRerolls());
			skipButton.gameObject.SetActive(PlayerStatsScaler.GetScaler().HasSkips());
			rerollButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Refresh ({PlayerStatsScaler.GetScaler().GetRerolls()})";
			skipButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Skip ({PlayerStatsScaler.GetScaler().GetSkips()})";
			var chanceOfAppearance = Random.value;
			var upgradesToPick = Random.Range(3, 5);

			var upgradeEntries = (_isWeaponOnly ? 
					WeaponManager.instance.GetWeaponUnlocks().Where(x => x.Weapon.AttackTypeField != AttackType.FollowUp) : WeaponManager.instance.GetUpgrades())
				.OrderByDescending(x => x.ChanceOfAppearance >= 1 - chanceOfAppearance)
				.ThenBy(_ => Random.value)
				.Take(upgradesToPick)
				.ToList();
			if (upgradeEntries.Count == 0)
			{
				ClosePanel();
				return;
			}
			
			if (GameSettings.IsRandomLevelUp)
			{
				Upgrade(upgradeEntries.OrderBy(_ => Random.value).First(), true);
				return;
			}

			if (TwitchIntegrationManager.instance.IsChatPicksItemsEnabled() && !TwitchPollManager.instance.IsAnyPoolRunning())
			{
				var pollChoices = upgradeEntries.Select(x => x.GetUnlockName()).ToArray();
				TwitchPollManager.instance.StartPoll("Pick upgrades", pollChoices, TwitchPollResult, upgradeEntries);
				_waitingForPollResult = true;
			}
			
			Clean();
			panel.SetActive(true);
			
			for (var i = 0; i < upgradeEntries.Count; i++)
			{
				upgradeButtons[i].gameObject.SetActive(true);
				upgradeButtons[i].Set(upgradeEntries[i], this);
			}
		}

		private void TwitchPollResult(int result, List<UpgradeEntry> upgradeEntries)
		{
			_waitingForPollResult = false;
			Upgrade(upgradeEntries[result], true);
		}

		public void Upgrade(UpgradeEntry upgradeEntry, bool ignoreWaitTime = false)
		{
			if (_waitingForPollResult) return;
			if (_timeOpened < 1.5f && !ignoreWaitTime) return;
			
			upgradeEntry.LevelUp(WeaponManager.instance);
			ClosePanel();
		}
	}
}