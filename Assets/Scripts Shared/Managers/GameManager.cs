using System;
using System.Collections.Generic;
using System.Linq;
using Data.Difficulty;
using DefaultNamespace;
using DefaultNamespace.Data;
using Managers.StageEvents;
using Objects.Characters;
using Objects.Players.PermUpgrades;
using Objects.Players.Scripts;
using Objects.Stage;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityTemplateProjects;

namespace Managers
{
	public class GameManager : MonoBehaviour
	{		
		[SerializeField] private GameResultData gameResultData;
		public static GameManager instance;
		private Transform _playerTransform;
		public Transform PlayerTransform
		{
			get
			{
				if (_playerTransform == null && NetworkManager.Singleton && NetworkManager.Singleton.LocalClient != null && NetworkManager.Singleton.LocalClient.PlayerObject)
					_playerTransform = NetworkManager.Singleton.LocalClient.PlayerObject.transform;
				return _playerTransform;
			}
		}

		private bool _isExiting;
		
		[SerializeField] public Player playerComponent;
		[HideInInspector] public PlayerVfxComponent playerVfxComponent;
		[SerializeField] public PlayerStatsComponent playerStatsComponent;
		[SerializeField] private SpecialBarManager specialBarManager;
		[SerializeField] private DifficultyContainer difficultyContainer;
		
		public void Awake()
		{
			if (instance == null)
			{
				instance = this;
			}

			Initialize();
		}
		
		private void Initialize()
		{
			var saveFile = FindFirstObjectByType<SaveFile>();
			GameData.SetCurrentDifficultyData(difficultyContainer.GetData(saveFile.SelectedDifficulty));
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
			
			FindFirstObjectByType<DiscordManager>().SetInGame();
		}

		public static bool IsCharacterState(PlayerCharacterState characterState)
		{
			return instance?.playerComponent?.CharacterState == characterState;
		}

		public void BackToMainMenu(bool isWin)
		{
			if (_isExiting) return;
			
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			_isExiting = true;
			gameResultData.IsGameEnd = true;
			gameResultData.IsWin = isWin;
			gameResultData.Level = FindFirstObjectByType<LevelComponent>()?.GetLevel() ?? 0;

			NetworkManager.Singleton.Shutdown(true);
			NetworkingContainer.IsHostPlayer = true;
			SceneManager.LoadScene("Scenes/Main Menu");
		}
	}
}