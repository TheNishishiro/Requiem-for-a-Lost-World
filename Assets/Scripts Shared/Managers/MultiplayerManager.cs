using System;
using System.Collections;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;
using UnityTemplateProjects;

namespace Managers
{
    public class MultiplayerManager : MonoBehaviour
    {
        public void StartClient()
        {
            NetworkManager.Singleton.StartClient();
            NetworkingContainer.IsHostPlayer = false;
        }

        public void SetNetworkIp(string ip)
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address=ip;
        }

        public void SetNetworkPort(string port)
        {
            if (ushort.TryParse(port, out var result))
            {
                NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Port = result;
            }
        }
    }
}