using System;
using System.Collections.Generic;
using Interfaces;
using UI.Main_Menu.Character_List_Menu;
using UnityEngine;

namespace UI.Labels.InGame.PauseMenu
{
	public class UnlockedUpgradesPanel : MonoBehaviour
	{
		[SerializeField] private WeaponManager weaponManager;
		[SerializeField] private GameObject upgradeEntryPrefab;
		[SerializeField] private Transform upgradeEntryContainer;
		[SerializeField] private bool isWeaponPanel;
		private List<UnlockedItemPanel> _unlockedItemPanels;

		public void UpdatePanel(StatsScrollMenuPanel statsScrollMenuPanel)
		{
			_unlockedItemPanels ??= new List<UnlockedItemPanel>();
			foreach (var unlockedItemPanel in _unlockedItemPanels)
			{
				Destroy(unlockedItemPanel.gameObject);
			}
			_unlockedItemPanels.Clear();

			var playerItems = isWeaponPanel ? weaponManager.GetUnlockedWeaponsAsInterface() : weaponManager.GetUnlockedItemsAsInterface();
			foreach (var item in playerItems)
			{
				var upgradeEntryGameObject = Instantiate(upgradeEntryPrefab, upgradeEntryContainer);
				var unlockedItemPanel = upgradeEntryGameObject.GetComponent<UnlockedItemPanel>();
				unlockedItemPanel.SetUpgradeEntry(item, statsScrollMenuPanel);
				_unlockedItemPanels.Add(unlockedItemPanel);
			}
		}
	}
}