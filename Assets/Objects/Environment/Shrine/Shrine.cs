using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UI.In_Game.GUI.Scripts.Managers;
using Unity.Netcode;
using UnityEngine;

public class Shrine : NetworkBehaviour
{
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float maxReviveTimer = 5f;
    [SerializeField] private Collider loadCollider;
    [SerializeField] private GameObject activeIndicator;
    private bool isLoadingRevive;
    private float reviveTimer;

    private void Update()
    {
        if (!isLoadingRevive) return;

        reviveTimer += Time.deltaTime;
        GuiManager.instance.UpdateReviveTime(reviveTimer, maxReviveTimer);
        if (reviveTimer >= maxReviveTimer)
        {
            RpcManager.instance.RevivePlayerServerRpc(GetComponent<NetworkObject>(), respawnPoint.position, GameManager.instance.playerComponent.GetActivePlayerCard());
            GuiManager.instance.SetReviveTimerVisible(false);
        }
    }

    public void MarkAsUsed()
    {
        activeIndicator.SetActive(false);
        loadCollider.enabled = false;
        enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsPlayer(other)) return;
        isLoadingRevive = true;
        GuiManager.instance.SetReviveTimerVisible(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsPlayer(other)) return;
        GuiManager.instance.SetReviveTimerVisible(false);
        isLoadingRevive = false;
        reviveTimer = 0;
    }

    private bool IsPlayer(Collider other)
    {
        if (!other.CompareTag("Player")) return false;
        if (GameManager.instance.playerStatsComponent.IsDead()) return false;
        if (other.GetComponent<NetworkObject>()?.IsOwner != true) return false;
        if (!GameManager.instance.playerComponent.HasPlayerCard()) return false;

        return true;
    }
}
