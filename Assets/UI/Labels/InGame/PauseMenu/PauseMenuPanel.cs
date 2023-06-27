using Managers;
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
			panel.SetActive(false);
		}
		
		public void OpenMenu()
		{
			cursorManager.ShowCursor();
			pauseManager.PauseGame();
			panel.SetActive(true);
			
			unlockedWeaponsPanel.UpdatePanel();
			unlockedItemsPanel.UpdatePanel();
		}
	}
}