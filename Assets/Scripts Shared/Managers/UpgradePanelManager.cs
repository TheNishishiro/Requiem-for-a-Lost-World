using System.Collections.Generic;
using System.Linq;
using Cinemachine;
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
	public class UpgradePanelManager : MonoBehaviour
	{
		[SerializeField] private GameObject panel;
		[SerializeField] private List<UpgradePanel> upgradeButtons;
		[SerializeField] private PauseManager pauseManager;
		[SerializeField] private WeaponManager weaponManager;
		[SerializeField] private CursorManager cursorManager;
		[SerializeField] private Button skipButton;
		[SerializeField] private Button rerollButton;
		[SerializeField] private PlayerStatsComponent playerStatsComponent;

		private void Start()
		{
			HideButtons();
			if (GameData.GetPlayerCharacterData().PickWeaponOnStart)
				OpenPickWeapon();
		}

		public void OpenPanel()
		{
			ReloadUpgrades();
		}

		public void OpenPickWeapon()
		{
			ReloadUpgrades(true);
		}

		public void ClosePanel()
		{
			cursorManager.HideCursor();
			HideButtons();
			pauseManager.UnPauseGame();
			panel.SetActive(false);
		}

		public void Clean()
		{
			foreach (var upgradeButton in upgradeButtons)
				upgradeButton.Clean();
		}

		public void HideButtons()
		{
			foreach (var upgradeButton in upgradeButtons)
				upgradeButton.gameObject.SetActive(false);
		}

		public void Reroll()
		{
			playerStatsComponent.IncreaseReroll(-1);
			rerollButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Reroll ({playerStatsComponent.GetRerolls()})";
			ReloadUpgrades();
		}

		public void Skip()
		{
			playerStatsComponent.IncreaseSkip(-1);
			skipButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Skip ({playerStatsComponent.GetSkips()})";
			ClosePanel();
		}

		private void ReloadUpgrades(bool isWeaponOnly = false)
		{
			HideButtons();
			rerollButton.gameObject.SetActive(playerStatsComponent.HasRerolls());
			skipButton.gameObject.SetActive(playerStatsComponent.HasSkips());
			var upgradeEntries = (isWeaponOnly ? weaponManager.GetWeaponUnlocks() : weaponManager.GetUpgrades())
				.OrderBy(_ => Random.value)
				.Take(Random.Range(3, 5))
				.ToList();
			if (upgradeEntries.Count == 0)
				return;
            
			cursorManager.ShowCursor();
			pauseManager.PauseGame();
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