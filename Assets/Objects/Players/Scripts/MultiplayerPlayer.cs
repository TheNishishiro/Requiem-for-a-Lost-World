using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Data.Difficulty;
using DefaultNamespace;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Cameras;
using DefaultNamespace.Data.Stages;
using Events.Handlers;
using Events.Scripts;
using Managers;
using Objects;
using Objects.Characters;
using Objects.Players.Containers;
using Objects.Players.Scripts;
using Objects.Stage;
using StarterAssets;
using UI.Labels.InGame.MP_List;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class MultiplayerPlayer : NetworkBehaviour, ISettingsChangedHandler
{
    public GameObject firstPersonCameraRoot;
    public GameObject thirdPersonCameraRoot;
    public PlayerInput playerInput;
    public FirstPersonController firstPersonController;
    public ThirdPersonController thirdPersonController;
    public StarterAssetsInputs starterAssetsInputs;
    public SpriteRenderer spriteRenderer;
    private CharactersEnum localCharacterId;
    private ulong localPlayerId;
    public NetworkVariable<CharactersEnum> currentCharacterId = new (0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> currentCharacterHealth = new (0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> currentCharacterMaxHealth = new (0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> currentCharacterLevel = new (0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<FixedString128Bytes> currentPlayerName = new ("", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<ulong> currentPlayerId = new (0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isPlayerDead = new (false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isVoteUnpause = new (false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<DifficultyEnum> difficulty = new ();
    public NetworkVariable<StageEnum> stage = new ();
    private bool _keepAlive = true;
    [SerializeField] private Transform targetPoint;
    [SerializeField] private Collider boxCollider;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private DifficultyContainer difficultyContainer;
    [SerializeField] private StageContainer stageContainer;
    [SerializeField] private PlayerDashComponent playerDashComponent;
    [SerializeField] private PlayerWeaponContainer playerWeaponContainer;
    
    public void Start()
    {
        tag = "Player";
    }

    public override void OnNetworkSpawn()
    {
        if (IsHost)
        {
            difficulty.Value = GameData.GetCurrentDifficulty().Difficulty;
            stage.Value = GameData.GetCurrentStage().id;
        }
        
        SetupCamera(GetRootCamera().transform);
        
        GameData.SetCurrentDifficultyData(difficultyContainer.GetData(difficulty.Value));
        GameData.SetCurrentStage(stageContainer.GetData(stage.Value));
        playerInput.enabled = IsOwner;
        playerDashComponent.enabled = IsOwner;
        
        starterAssetsInputs.enabled = IsOwner;
        base.OnNetworkSpawn();
        
        if (IsOwner)
        {
            transform.position = FindFirstObjectByType<SpawnPoint>().transform.position;
            StartCoroutine(WaitForAddWeapon());
            StartCoroutine(WaitForCursorManager());
            StartCoroutine(WaitForEnemyManager());
            StartCoroutine(WaitForGameManager());
            StartCoroutine(WaitForPlayerSkillComponent());
            StartCoroutine(WaitScreenControlsManager());

            currentCharacterId.Value = GameData.GetPlayerCharacterId();
        }
    }

    public void VoteUnpause()
    {
        if (!IsOwner) return;

        isVoteUnpause.Value = true;
    }

    public void ResetVoteUnpause()
    {
        if (!IsOwner) return;

        isVoteUnpause.Value = false;
    }

    private void SetupCamera(Transform targetTransform)
    {
        if (IsOwner && GetRootCamera() != null)
            SetCameraTarget(isPlayerDead.Value ? targetTransform : GetRootCamera().transform);
        
        firstPersonController.enabled = SaveFile.Instance.CameraMode is CameraModes.StaticThirdPerson or CameraModes.FirstPerson && IsOwner;
        thirdPersonController.enabled = SaveFile.Instance.CameraMode is CameraModes.FreeThirdPerson or CameraModes.TopDown && IsOwner;
        switch (SaveFile.Instance.CameraMode)
        {
            case CameraModes.TopDown when IsOwner:
                thirdPersonController.TopClamp = 50;
                thirdPersonController.BottomClamp = 50;
                break;
            case CameraModes.FirstPerson when IsOwner:
                firstPersonController.TopClamp = 30;
                firstPersonController.BottomClamp = -30;
                break;
            case CameraModes.StaticThirdPerson when IsOwner:
                firstPersonController.TopClamp = 30;
                firstPersonController.BottomClamp = -30;
                break;
            case CameraModes.FreeThirdPerson when IsOwner:
                thirdPersonController.TopClamp = 40;
                thirdPersonController.BottomClamp = -30;
                break;
        }
    }

    public void SetCameraTarget(Transform targetTransform)
    {
        GameManager.instance.GetCameraInstance().Follow = targetTransform;
        GameManager.instance.GetCameraInstance().LookAt = targetTransform;
    }

    public void ResetCameraFollow()
    {
        SetCameraTarget(GetRootCamera().transform);
    }

    public GameObject GetRootCamera()
    {
        return SaveFile.Instance.CameraMode switch
        {
            CameraModes.StaticThirdPerson => firstPersonCameraRoot,
            CameraModes.FreeThirdPerson => thirdPersonCameraRoot,
            CameraModes.TopDown => thirdPersonCameraRoot,
            CameraModes.FirstPerson => firstPersonCameraRoot,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public void Update()
    {
        if (IsOwner)
        {
            var saveFile = FindFirstObjectByType<SaveFile>();
            currentCharacterHealth.Value = PlayerStatsScaler.GetScaler().GetHealth();
            currentCharacterMaxHealth.Value = PlayerStatsScaler.GetScaler().GetMaxHealth();
            currentCharacterLevel.Value = GameManager.instance.playerComponent.GetLevel();
            currentPlayerId.Value = NetworkManager.Singleton.LocalClientId;
            currentPlayerName.Value = saveFile.ConfigurationFile.Username ?? string.Empty;
            isPlayerDead.Value = GameManager.instance.playerStatsComponent.IsDead();
        }

        if (localCharacterId != currentCharacterId.Value)
        {
            spriteRenderer.sprite = GameData.GetCharacterSprite(currentCharacterId.Value);
            localCharacterId = currentCharacterId.Value;
            localPlayerId = currentPlayerId.Value;
            
            if (!IsOwner)
                MpActivePlayersInGameList.instance.UpdateEntryAvatar(currentPlayerId.Value, GameData.GetCharacterAvatar(currentCharacterId.Value));
        }

        if (!IsOwner)
        {
            MpActivePlayersInGameList.instance.UpdateEntry(currentPlayerId.Value, currentCharacterHealth.Value, currentCharacterMaxHealth.Value, currentPlayerName.Value.ToString(), currentCharacterLevel.Value);
        }
        
        spriteRenderer.enabled = !isPlayerDead.Value;
    }

    public void SetCollider(bool isEnabled)
    {
        boxCollider.enabled = isEnabled;
        characterController.enabled = isEnabled;
    }

    public Transform GetTargetPoint()
    {
        return targetPoint;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner)
        {
            MpActivePlayersInGameList.instance.RemoveEntry(localPlayerId);
        }
        if (IsOwner)
        {
            _keepAlive = false;
            GameManager.instance.BackToMainMenu();
        }
    }

    private IEnumerator WaitForAddWeapon()
    {
        while (true)
        {
            if (WeaponManager.instance != null && WeaponManager.instance.isInitialized && RpcManager.instance != null)
            {
                WeaponManager.instance.AddStartingWeapon(playerWeaponContainer.transform);
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator WaitForGameManager()
    {
        while (true)
        {
            if (GameManager.instance != null)
            {
                GameManager.instance.playerVfxComponent = GetComponent<PlayerVfxComponent>();
                GameManager.instance.playerMpComponent = this;
                yield break;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator WaitForPlayerSkillComponent()
    {
        while (true)
        {
            if (PlayerSkillComponent.instance != null)
            {
                PlayerSkillComponent.instance.Init(GetComponentInChildren<PlayerAbilityContainer>().transform);
                yield break;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator WaitForEnemyManager()
    {
        while (true)
        {
            if (EnemyManager.instance != null)
            {
                yield return new WaitForSeconds(2);
                EnemyManager.instance.IsDisableEnemySpawn = false;
                yield break;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator WaitForCursorManager()
    {
        while (true)
        {
            if (CursorManager.instance != null && WeaponManager.instance != null)
            {
                CursorManager.instance.Setup(GetComponentInChildren<StarterAssetsInputs>());
                
                var upgradePanelManager = FindFirstObjectByType<UpgradePanelManager>();
                    
                if (GameData.GetPlayerCharacterData().PickWeaponOnStart)
                    upgradePanelManager.OpenPickWeapon();
                yield break;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator WaitScreenControlsManager()
    {
        while (true)
        {
            var touchScreenControllerInput = FindFirstObjectByType<UICanvasControllerInput>();
            if (touchScreenControllerInput != null)
            {
                touchScreenControllerInput.SetInputAsset(GetComponentInChildren<StarterAssetsInputs>());
                yield break;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void OnSettingsChanged()
    {
        if (!IsOwner) return;
        var followPoint = GameManager.instance.GetCameraInstance().Follow;
        if (followPoint == null)
            followPoint = GetRootCamera().transform;
        SetupCamera(followPoint);
    }

    private void OnEnable()
    {
        SettingsChangedEvent.Register(this);
    }

    private void OnDisable()
    {
        SettingsChangedEvent.Unregister(this);
    }
}
