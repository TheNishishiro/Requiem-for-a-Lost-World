using Managers;
using Objects.Players.Scripts;
using Objects.Stage;
using UI.Main_Menu.Character_List_Menu;
using UnityEngine;

namespace UI.Labels.InGame.PauseMenu
{
	public class PauseMenuPanel : MonoBehaviour
	{
		[SerializeField] private GameObject panel;
		[SerializeField] private GameObject levelUpPanel;
		[SerializeField] private GameObject chestPanel;
		[SerializeField] private GameObject gameOverPanel;
		[SerializeField] private CursorManager cursorManager;
		[SerializeField] private PauseManager pauseManager;
		[SerializeField] private UnlockedUpgradesPanel unlockedWeaponsPanel;
		[SerializeField] private UnlockedUpgradesPanel unlockedItemsPanel;
		[SerializeField] private GameObject statsPanel;
		[SerializeField] private GameObject weaponStatsPanel;
		[SerializeField] private PlayerStatsComponent playerStatsComponent;
		

		private void Update()
		{
			if (!Input.GetKeyDown(KeyCode.Escape)) return;
			
			if (!panel.activeInHierarchy && !levelUpPanel.activeInHierarchy && !chestPanel.activeInHierarchy && !gameOverPanel.activeInHierarchy)
				OpenMenu();
			else
				CloseMenu();
		}

		public void CloseMenu()
		{
			cursorManager.HideCursor();
			pauseManager.UnPauseGame();
			
			var statsPanelComponent = statsPanel.GetComponent<StatsScrollMenuPanel>();
			statsPanelComponent.ClearEntries();
			panel.SetActive(false);
		}
		
		public void OpenMenu()
		{
			cursorManager.ShowCursor();
			pauseManager.PauseGame();
			panel.SetActive(true);
			
			var statsPanelComponent = statsPanel.GetComponent<StatsScrollMenuPanel>();
			statsPanelComponent.ClearEntries();
			foreach (var statEntry in playerStatsComponent.GetStats())
			{
				statsPanelComponent.AddEntry(statEntry.Name, statEntry.Value);
			}
			
			var weaponStatsComponent = weaponStatsPanel.GetComponent<StatsScrollMenuPanel>();
			unlockedWeaponsPanel.UpdatePanel(weaponStatsComponent);
			unlockedItemsPanel.UpdatePanel(weaponStatsComponent);
		}
	}
}