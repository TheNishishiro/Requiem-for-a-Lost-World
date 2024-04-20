using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Managers;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityTemplateProjects;

public class MultiplayerController : MonoBehaviour
{
    void Update()
    {
        if (NetworkManager.Singleton.ShutdownInProgress) return;
        if (!NetworkingContainer.IsHostPlayer) return;

        NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
        NetworkManager.Singleton.StartHost();
        NetworkingContainer.IsHostPlayer = false;
    }
    
    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        if (NetworkingContainer.IsHostPlayer)
        {
            response.Approved = true;
            response.CreatePlayerObject = true;
            response.PlayerPrefabHash = null;
            response.Pending = false;
            return;
        }

        response.Approved = NetworkingContainer.IsAllowJoins;
        response.CreatePlayerObject = true;
        response.PlayerPrefabHash = null;
        response.Pending = false;
        response.Reason = "Host does not accept CO-OP requests.";
    }
}
