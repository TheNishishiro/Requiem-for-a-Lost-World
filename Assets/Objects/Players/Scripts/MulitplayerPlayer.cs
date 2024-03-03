using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Managers;
using Objects.Characters;
using Objects.Players.Containers;
using Objects.Players.Scripts;
using Objects.Stage;
using StarterAssets;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class MulitplayerPlayer : NetworkBehaviour
{
    public GameObject cameraRoot;
    public PlayerInput playerInput;
    public FirstPersonController firstPersonController;
    public StarterAssetsInputs starterAssetsInputs;
    public SpriteRenderer spriteRenderer;
    private CharactersEnum localCharacterId;
    public NetworkVariable<CharactersEnum> currentCharacterId = new (0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
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

            currentCharacterId.Value = GameData.GetPlayerCharacterId();
        }
    }

    public void Update()
    {
        if (localCharacterId != currentCharacterId.Value)
        {
            spriteRenderer.sprite = GameData.GetCharacterSprite(currentCharacterId.Value);
            localCharacterId = currentCharacterId.Value;
        }
    }

    public override void OnNetworkDespawn()
    {
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
            if (cM != null)
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
}
