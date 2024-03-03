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
        NetworkManager.Singleton.StartHost();
        NetworkingContainer.IsHostPlayer = false;
    }
}
