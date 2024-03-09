using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
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
        GameManager.instance.reviveTimerBar.UpdateSlider(reviveTimer, maxReviveTimer);
        if (reviveTimer >= maxReviveTimer)
        {
            RpcManager.instance.RevivePlayerServerRpc(GetComponent<NetworkObject>(), respawnPoint.position, GameManager.instance.playerComponent.GetActivePlayerCard());
            GameManager.instance.reviveTimerBar.gameObject.SetActive(false);
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
        GameManager.instance.reviveTimerBar.gameObject.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsPlayer(other)) return;
        GameManager.instance.reviveTimerBar.gameObject.SetActive(false);
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
