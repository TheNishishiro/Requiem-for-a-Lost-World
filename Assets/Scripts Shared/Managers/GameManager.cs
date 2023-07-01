using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Data;
using Objects.Players.PermUpgrades;
using Objects.Players.Scripts;
using Objects.Stage;
using UnityEngine;

namespace Managers
{
	public class GameManager : MonoBehaviour
	{
		[SerializeField] private PlayerStatsComponent playerStatsComponent;
		private SaveFile _saveFile;
		
		private void Start()
		{
			_saveFile = FindObjectOfType<SaveFile>();
			playerStatsComponent.Set(GameData.GetPlayerStartingStats());
			
			
			var permUpgrades = GameData.GetPermUpgrades().ToList();
			foreach (var permUpgradesSaveData in _saveFile.PermUpgradeSaveData ?? new Dictionary<PermUpgradeType, int>())
			{
				var permUpgrade = permUpgrades.FirstOrDefault(x => x.type == permUpgradesSaveData.Key);
				if (permUpgrade != null)
				{
					playerStatsComponent.ApplyPermanent(permUpgrade, permUpgradesSaveData.Value);
				}
			}
		}
	}
}