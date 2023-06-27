using System.Collections.Generic;
using System.Linq;
using UI.Labels.InGame.LevelUpScreen;
using UnityEngine;
using UpgradePanel = UI.Labels.InGame.UpgradeScreen.UpgradePanel;

namespace Managers
{
	public class ChestPanelManager : MonoBehaviour
	{
		[SerializeField] private List<UpgradePanel> upgradePanels;
		[SerializeField] private WeaponManager weaponManager;
		[SerializeField] private GameObject panel;
		[SerializeField] private PauseManager pauseManager;
		[SerializeField] private CursorManager cursorManager;
		
		public void OpenPanel()
		{
			var upgradeEntries = new List<UpgradeEntry>();
			upgradeEntries.AddRange(weaponManager.GetWeaponUpgrades());
			upgradeEntries.AddRange(weaponManager.GetItemUpgrades());

			var chance = Random.value;
			var amount = chance switch
			{
				< 0.01f => 5,
				< 0.1f => 3,
				_ => 1
			};

			upgradeEntries = upgradeEntries.OrderBy(x => Random.value).Take(amount).ToList();
			if (upgradeEntries.Count == 0)
			{
				var player = FindObjectOfType<Player>();
				player.AddGold(Random.Range(1, 50));
				player.AddGems(Random.Range(1, 10));
				return;
			}
				
			
			cursorManager.ShowCursor();
			pauseManager.PauseGame();
			Clean();
			panel.SetActive(true);
			
			for (var i = 0; i < upgradeEntries.Count; i++)
			{
				upgradePanels[i].gameObject.SetActive(true);
				upgradePanels[i].SetUpgradeData(upgradeEntries[i]);
				upgradeEntries[i].LevelUp(weaponManager);
			}
		}
		
		public void ClosePanel()
		{
			HideButtons();
			pauseManager.UnPauseGame();
			panel.SetActive(false);
			cursorManager.HideCursor();
		}
		
		public void HideButtons()
		{
			foreach (var upgradePanel in upgradePanels)
				upgradePanel.gameObject.SetActive(false);
		}
		
		public void Clean()
		{
			foreach (var upgradePanel in upgradePanels)
				upgradePanel.Clean();
		}
	}
}