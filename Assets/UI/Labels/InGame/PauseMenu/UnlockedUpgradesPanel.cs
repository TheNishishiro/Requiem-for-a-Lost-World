using System;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace UI.Labels.InGame.PauseMenu
{
	public class UnlockedUpgradesPanel : MonoBehaviour
	{
		[SerializeField] private WeaponManager weaponManager;
		[SerializeField] private GameObject upgradeEntryPrefab;
		[SerializeField] private bool isWeaponPanel;
		private List<UnlockedItemPanel> _unlockedItemPanels;

		public void UpdatePanel()
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
				var upgradeEntryGameObject = Instantiate(upgradeEntryPrefab, transform);
				var unlockedItemPanel = upgradeEntryGameObject.GetComponent<UnlockedItemPanel>();
				unlockedItemPanel.SetUpgradeEntry(item);
				_unlockedItemPanels.Add(unlockedItemPanel);
			}
		}
	}
}