using Managers;
using Objects.Players.Scripts;
using Objects.Stage;
using UI.Main_Menu.Character_List_Menu;
using Unity.Netcode;
using UnityEngine;

namespace UI.Labels.InGame.PauseMenu
{
	public class PauseMenuPanel : MonoBehaviour
	{
		[SerializeField] private GameObject panel;
		[SerializeField] private GameObject levelUpPanel;
		[SerializeField] private GameObject chestPanel;
		[SerializeField] private GameObject gameOverPanel;
		[SerializeField] private PauseManager pauseManager;
		[SerializeField] private UnlockedUpgradesPanel unlockedWeaponsPanel;
		[SerializeField] private UnlockedUpgradesPanel unlockedItemsPanel;
		[SerializeField] private GameObject statsPanel;
		[SerializeField] private GameObject weaponStatsPanel;
		[SerializeField] private PlayerStatsComponent playerStatsComponent;

		private void Update()
		{
			if (!Input.GetKeyDown(KeyCode.Escape)) return;

			ToggleMenu();
		}

		public void ToggleMenu()
		{
			if (levelUpPanel.activeInHierarchy || chestPanel.activeInHierarchy || gameOverPanel.activeInHierarchy) return;
			if (!panel.activeInHierarchy)
				OpenMenu();
			else
				CloseMenu();
		}

		public void CloseMenu()
		{
			pauseManager.UnPauseGame();
			
			var statsPanelComponent = statsPanel.GetComponent<StatsScrollMenuPanel>();
			statsPanelComponent.ClearEntries();
			panel.SetActive(false);
		}
		
		public void OpenMenu()
		{
			pauseManager.PauseGame();
			panel.SetActive(true);
			
			var statsPanelComponent = statsPanel.GetComponent<StatsScrollMenuPanel>();
			statsPanelComponent.ClearEntries();
			foreach (var statEntry in playerStatsComponent.GetStatsDisplayData())
			{
				statsPanelComponent.AddEntry(statEntry.Name, statEntry.Value, statEntry.Description);
			}
			
			var weaponStatsComponent = weaponStatsPanel.GetComponent<StatsScrollMenuPanel>();
			unlockedWeaponsPanel.UpdatePanel(weaponStatsComponent);
			unlockedItemsPanel.UpdatePanel(weaponStatsComponent);
		}
	}
}