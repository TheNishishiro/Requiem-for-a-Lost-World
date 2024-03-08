using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DefaultNamespace.Data;
using Managers;
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

public class MultiplayerPlayer : NetworkBehaviour
{
    public GameObject cameraRoot;
    public PlayerInput playerInput;
    public FirstPersonController firstPersonController;
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
    private bool _keepAlive = true;
    
    public void Start()
    {
        tag = "Player";
        transform.position = new Vector3(0, 6, 0);
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner && cameraRoot != null)
            FindFirstObjectByType<CinemachineVirtualCamera>().Follow = cameraRoot.transform;
        playerInput.enabled = IsOwner;
        firstPersonController.enabled = IsOwner;
        starterAssetsInputs.enabled = IsOwner;
        base.OnNetworkSpawn();
        
        if (IsOwner)
        {
            StartCoroutine(WaitForAddWeapon());
            StartCoroutine(WaitForCursorManager());
            StartCoroutine(WaitForEnemyManager());
            StartCoroutine(WaitForGameManager());
            StartCoroutine(WaitForPlayerSkillComponent());
            StartCoroutine(WaitScreenControlsManager());

            currentCharacterId.Value = GameData.GetPlayerCharacterId();
        }
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
            if (WeaponManager.instance != null)
            {
                WeaponManager.instance.AddStartingWeapon(GetComponentInChildren<PlayerWeaponContainer>().transform);
                yield break;
            }

            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator WaitForGameManager()
    {
        while (true)
        {
            if (GameManager.instance != null)
            {
                GameManager.instance.playerVfxComponent = GetComponent<PlayerVfxComponent>();
                yield break;
            }

            yield return new WaitForSeconds(1);
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

            yield return new WaitForSeconds(1);
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

            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator WaitForCursorManager()
    {
        while (true)
        {
            var cM = FindFirstObjectByType<CursorManager>();
            if (cM != null && WeaponManager.instance != null)
            {
                cM.Setup(GetComponentInChildren<StarterAssetsInputs>());
                
                var upgradePanelManager = FindFirstObjectByType<UpgradePanelManager>();
                    
                if (GameData.GetPlayerCharacterData().PickWeaponOnStart)
                    upgradePanelManager.OpenPickWeapon();
                yield break;
            }

            yield return new WaitForSeconds(1);
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

            yield return new WaitForSeconds(1);
        }
    }
}
