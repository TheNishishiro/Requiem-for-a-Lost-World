using System;
using System.Collections.Generic;
using System.Globalization;
using DefaultNamespace.Data;
using Objects.Players.PermUpgrades;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_Menu.Upgrades_Menu
{
	public class UpgradeEntryPanel : MonoBehaviour
	{
		[SerializeField] TextMeshProUGUI upgradeName;
		[SerializeField] TextMeshProUGUI upgradeDescription;
		[SerializeField] TextMeshProUGUI upgradeCost;
		[SerializeField] private Image image;
		private SaveFile _saveFile;
		private PermUpgrade _permUpgrade;
		private int _upgradeLevel;
		private int _currentLevel;

		private void Awake()
		{
			_saveFile = FindObjectOfType<SaveFile>();
		}

		public void Set(int upgradeLevel, PermUpgrade upgrade)
		{
			_permUpgrade = upgrade;
			_currentLevel = upgradeLevel;
		}

		public void Update()
		{
			if (_permUpgrade == null)
				return;
			
			upgradeName.text = $"{_permUpgrade.name} {_currentLevel}/{_permUpgrade.maxLevel}";
			upgradeDescription.text = _permUpgrade.description;
			upgradeCost.text = _currentLevel == _permUpgrade.maxLevel ? "MAX" : $"{_permUpgrade.costPerLevel * (_currentLevel+1)}G";
			image.sprite = _permUpgrade.icon;
		}

		public void Upgrade()
		{
			if (_currentLevel >= _permUpgrade.maxLevel || _saveFile.Gold < (ulong)(_permUpgrade.costPerLevel * (_currentLevel+1)))
				return;

			_saveFile.Gold -= (ulong)(_permUpgrade.costPerLevel * (_currentLevel+1));
			_saveFile.AddUpgradeLevel(_permUpgrade.type);
			if (_permUpgrade.type == PermUpgradeType.BuyGems)
				_saveFile.Gems += (ulong)_permUpgrade.increasePerLevel;
			
			_currentLevel++;
			_saveFile.Save();
		}
	}
}