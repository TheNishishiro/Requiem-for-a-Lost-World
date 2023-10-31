using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using DefaultNamespace.Data;
using Managers.StageEvents;
using Objects.Characters;
using Objects.Players.PermUpgrades;
using Objects.Players.Scripts;
using Objects.Stage;
using UnityEngine;

namespace Managers
{
	public class GameManager : MonoBehaviour
	{
		public static GameManager instance;
		[SerializeField] public PlayerStatsComponent playerStatsComponent;
		[SerializeField] public Player playerComponent;
		[SerializeField] private SpecialBarManager specialBarManager;

		private void Start()
		{
			if (instance == null)
			{
				instance = this;
			}
			
			var saveFile = FindObjectOfType<SaveFile>();
			playerStatsComponent.Set(GameData.GetPlayerStartingStats());
            if (GameData.GetPlayerCharacterData()?.UseSpecialBar == true)
	            specialBarManager.gameObject.SetActive(true);
			
			var permUpgrades = GameData.GetPermUpgrades().ToList();
			foreach (var permUpgradesSaveData in saveFile.PermUpgradeSaveData ?? new Dictionary<PermUpgradeType, int>())
			{
				var permUpgrade = permUpgrades.FirstOrDefault(x => x.type == permUpgradesSaveData.Key);
				if (permUpgrade != null)
				{
					playerStatsComponent.ApplyPermanent(permUpgrade, permUpgradesSaveData.Value);
				}
			}
			
			FindObjectOfType<DiscordManager>().SetInGame();
		}
	}
}