using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using Unity.Netcode;
using UnityEngine;

public class ReviveCard : NetworkBehaviour
{
    public NetworkVariable<ulong> clientId = new ();
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (GameManager.instance.playerStatsComponent.IsDead()) return;
        if (other.GetComponent<NetworkObject>()?.IsOwner != true) return;

        GameManager.instance.playerComponent.AddPlayerCard(clientId.Value);
        RpcManager.instance.DeSpawnReviveCardRpc(this);
    }

    public void SetClientId(ulong id)
    {
        if (IsHost)
            this.clientId.Value = id;
    }
}
