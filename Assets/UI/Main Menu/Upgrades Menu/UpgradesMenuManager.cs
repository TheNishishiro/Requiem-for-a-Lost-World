using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Data;
using Managers;
using UnityEngine;

namespace UI.Main_Menu.Upgrades_Menu
{
	public class UpgradesMenuManager : MonoBehaviour
	{
		[SerializeField] private GameObject panel;
		[SerializeField] private GameObject container;
		[SerializeField] private UpgradeEntryPanel upgradeEntryPanelPrefab;
		private List<UpgradeEntryPanel> _upgradeEntryPanels = new ();
		private SaveFile _saveFile;

		private void Awake()
		{
			_saveFile = FindObjectOfType<SaveFile>();
		}

		public void Open()
		{
			panel.SetActive(true);
		
			var permUpgrades = PermUpgradeListManager.instance.GetUpgrades();
			foreach (var permUpgrade in permUpgrades)
			{
				var upgradePanel = Instantiate(upgradeEntryPanelPrefab, container.transform);
				_upgradeEntryPanels.Add(upgradePanel);

				var upgradeLevel = 0;
				if (_saveFile.PermUpgradeSaveData?.TryGetValue(permUpgrade.type, out upgradeLevel) == true)
				{
					upgradePanel.Set(upgradeLevel, permUpgrade);
					continue;
				}
				
				upgradePanel.Set(0, permUpgrade);
			}
		}
	
		public void Close()
		{
			panel.SetActive(false);
			foreach (var upgradeEntryPanel in _upgradeEntryPanels)
			{
				Destroy(upgradeEntryPanel.gameObject);
			}
			_upgradeEntryPanels.Clear();
		}
		
		public void Toggle()
		{
			if (panel.activeSelf)
			{
				Close();
				return;
			}
			Open();
		}
	}
}