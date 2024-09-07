using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Data.Difficulty;
using Data.Elements;
using DefaultNamespace;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Cameras;
using DefaultNamespace.Data.Game;
using Events.Handlers;
using Events.Scripts;
using Managers.StageEvents;
using Objects;
using Objects.Characters;
using Objects.Players.PermUpgrades;
using Objects.Players.Scripts;
using Objects.Stage;
using UI.Labels.InGame;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityTemplateProjects;

namespace Managers
{	public class GameManager : MonoBehaviour, ISettingsChangedHandler, IDamageDealtHandler
	{		
		public static GameManager instance;
		[HideInInspector] public MultiplayerPlayer playerMpComponent;
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
		[SerializeField] public StatusEffectManager statusEffectManager;
		[SerializeField] private CinemachineVirtualCamera firstPersonVirtualCamera;
		[SerializeField] private CinemachineVirtualCamera thirdPersonVirtualCamera;
		private StageTime _stageTime;
		[HideInInspector] public SaveFile saveFile;
		[HideInInspector] public bool IsPlayerSprinting;

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
			_stageTime = FindFirstObjectByType<StageTime>();
			saveFile = FindFirstObjectByType<SaveFile>();
			SetupPlayerCamera();

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
		}

		public float GetGameTimeInMinutes()
		{
			return Utilities.GetTimeSpan(_stageTime.time.Value).Minutes;
		}

		public void OnSettingsChanged()
		{
			SetupPlayerCamera();
		}

		private void OnEnable()
		{
			SettingsChangedEvent.Register(this);
			DamageDealtEvent.Register(this);
		}

		private void OnDisable()
		{
			SettingsChangedEvent.Unregister(this);
			DamageDealtEvent.Unregister(this);
		}

		private void SetupPlayerCamera()
		{
			firstPersonVirtualCamera.gameObject.SetActive(saveFile.CameraMode is CameraModes.StaticThirdPerson or CameraModes.FirstPerson);
			thirdPersonVirtualCamera.gameObject.SetActive(saveFile.CameraMode is CameraModes.FreeThirdPerson or CameraModes.TopDown);
			switch (saveFile.CameraMode)
			{
				case CameraModes.TopDown:
				{
					var componentBase = thirdPersonVirtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
					if (componentBase is Cinemachine3rdPersonFollow follow)
					{
						follow.CameraDistance = 7;
					}

					break;
				}
				case CameraModes.FirstPerson:
				{
					firstPersonVirtualCamera.m_Lens.FieldOfView = 80;
					var componentBase = firstPersonVirtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
					if (componentBase is Cinemachine3rdPersonFollow follow)
					{
						follow.CameraDistance = 0;
					}

					break;
				}
				case CameraModes.StaticThirdPerson:
				{
					firstPersonVirtualCamera.m_Lens.FieldOfView = 50;
					var componentBase = firstPersonVirtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
					if (componentBase is Cinemachine3rdPersonFollow follow)
					{
						follow.CameraDistance = 3;
					}

					break;
				}
				case CameraModes.FreeThirdPerson:
				{
					thirdPersonVirtualCamera.m_Lens.FieldOfView = 40;
					var componentBase = thirdPersonVirtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
					if (componentBase is Cinemachine3rdPersonFollow follow)
					{
						follow.CameraDistance = 4;
					}

					break;
				}
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public static bool IsCharacterState(PlayerCharacterState characterState)
		{
			return instance?.playerComponent?.CharacterState == characterState;
		}

		public void BackToMainMenu()
		{
			GameFinishedEvent.Invoke(GameResultData.IsWin);
			BackToMainMenu(GameResultData.IsWin);
		}

		public void ToggleSprint()
		{
			IsPlayerSprinting = !IsPlayerSprinting;
		}
		
		public void BackToMainMenu(bool isWin)
		{
			if (_isExiting) return;
			
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			_isExiting = true;
			GameResultData.IsGameEnd = true;
			GameResultData.IsWin = isWin;
			GameResultData.Level = FindFirstObjectByType<LevelComponent>()?.GetLevel() ?? 0;

			NetworkManager.Singleton.Shutdown(true);
			NetworkingContainer.IsHostPlayer = true;
			SceneManager.LoadScene("Scenes/Main Menu");
		}

		public CinemachineVirtualCamera GetCameraInstance()
		{
			return SaveFile.Instance.CameraMode switch
			{
				CameraModes.StaticThirdPerson => firstPersonVirtualCamera,
				CameraModes.FreeThirdPerson => thirdPersonVirtualCamera,
				CameraModes.TopDown => thirdPersonVirtualCamera,
				CameraModes.FirstPerson => firstPersonVirtualCamera,
				_ => throw new ArgumentOutOfRangeException()
			};
		}

		public void OnDamageDealt(Damageable damageable, float damage, bool isRecursion, WeaponEnum weaponId)
		{
			GameResultData.AddDamage(damage, WeaponManager.instance.GetUnlockedWeapon(weaponId));
		}
	}
}