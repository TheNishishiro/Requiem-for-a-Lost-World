using System;
using System.Collections;
using System.Net;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;
using UnityTemplateProjects;

namespace Managers
{
    public class MultiplayerManager : MonoBehaviour
    {
        [SerializeField] private GameObject failedMessage;
        [SerializeField] private GameObject connectingMessage;
        private bool _loadingClient;

        private void Awake()
        {
            NetworkManager.Singleton.OnClientConnectedCallback += SingletonOnOnClientConnectedCallback;
            NetworkManager.Singleton.OnClientDisconnectCallback += SingletonOnOnClientDisconnectCallback;
        }

        private void OnDestroy()
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= SingletonOnOnClientConnectedCallback;
            NetworkManager.Singleton.OnClientDisconnectCallback -= SingletonOnOnClientDisconnectCallback;
        }

        public void StartClient()
        {
            if (!_loadingClient)
                StartCoroutine(StartClientCoroutine());
        }

        public IEnumerator StartClientCoroutine()
        {
            try
            {
                _loadingClient = true;
                while (NetworkManager.Singleton.ShutdownInProgress) ;

                failedMessage.SetActive(false);
                connectingMessage.SetActive(true);
                NetworkManager.Singleton.GetComponent<UnityTransport>().MaxConnectAttempts = 10;
                NetworkManager.Singleton.StartClient();
                NetworkingContainer.IsHostPlayer = false;
                yield break;
            }
            finally
            {
                _loadingClient = false;
            }
        }

        private void SingletonOnOnClientDisconnectCallback(ulong obj)
        {
            connectingMessage.SetActive(false);
            failedMessage.SetActive(true);
            _loadingClient = false;
            if (!NetworkManager.Singleton.ShutdownInProgress)
                NetworkManager.Singleton.Shutdown();
        }

        private void SingletonOnOnClientConnectedCallback(ulong obj)
        {
            connectingMessage.SetActive(false);
            failedMessage.SetActive(false);
            _loadingClient = false;
            NetworkManager.Singleton.GetComponent<UnityTransport>().MaxConnectAttempts = 60;
        }

        public void SetNetworkIp(string ip)
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address = IPAddress.TryParse(ip, out _) ? ip : "127.0.0.1";
        }

        public void SetNetworkPort(string port)
        { 
            NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Port = ushort.TryParse(port, out var result) ? result : (ushort)7777;
        }
    }
}