using System.Collections.Generic;
using System.Linq;
using Interfaces;
using UI.Labels.InGame.LevelUpScreen;
using UnityEngine;
using UpgradePanel = UI.Labels.InGame.UpgradeScreen.UpgradePanel;

namespace Managers
{
	public class ChestPanelManager : MonoBehaviour, IQueueableWindow
	{
		[SerializeField] private List<UpgradePanel> upgradePanels;
		[SerializeField] private WeaponManager weaponManager;
		[SerializeField] private GameObject panel;
		
		public void OpenPanel()
		{
			WindowManager.instance.QueueWindow(this);
		}
		
		public void ClosePanel()
		{
			WindowManager.instance.DeQueueWindow();
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

		public void Open()
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
				var player = GameManager.instance.playerComponent;
				player.AddGold(Random.Range(1, 50));
				player.AddGems(Random.Range(1, 10));
				ClosePanel();
				return;
			}
			
			Clean();
			panel.SetActive(true);
			
			for (var i = 0; i < upgradeEntries.Count; i++)
			{
				upgradePanels[i].gameObject.SetActive(true);
				upgradePanels[i].SetUpgradeData(upgradeEntries[i]);
				upgradeEntries[i].LevelUp(weaponManager);
			}
		}

		public void Close()
		{
			HideButtons();
			panel.SetActive(false);
		}
	}
}