using System;
using System.Collections;
using System.Net;
using DefaultNamespace.Attributes;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Modals;
using DefaultNamespace.Steam;
using Objects.Stage;
using UI.Main_Menu.REWORK.Scripts;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityTemplateProjects;

namespace Managers
{
    public class MultiplayerManager : MonoBehaviour
    {
        private bool _loadingClient;

        private void Awake()
        {
            if (NetworkManager.Singleton == null)
                return;
            
            NetworkManager.Singleton.OnClientConnectedCallback += SingletonOnOnClientConnectedCallback;
            NetworkManager.Singleton.OnClientDisconnectCallback += SingletonOnOnClientDisconnectCallback;
            NetworkManager.Singleton.OnTransportFailure += SingletonOnOnTransportFailure;
        }

        private void OnDestroy()
        {
            Debug.Log("MultiplayerManager::OnDestroy");
            if (NetworkManager.Singleton == null)
                return;
            
            NetworkManager.Singleton.OnClientConnectedCallback -= SingletonOnOnClientConnectedCallback;
            NetworkManager.Singleton.OnClientDisconnectCallback -= SingletonOnOnClientDisconnectCallback;
            NetworkManager.Singleton.OnTransportFailure -= SingletonOnOnTransportFailure;
        }

        public void StartClient()
        {
            if (!_loadingClient)
                StartCoroutine(StartClientCoroutine());
        }

        private void SingletonOnOnTransportFailure()
        {
            var reason = NetworkManager.Singleton.DisconnectReason;
            Debug.Log(reason);
            ModalManager.instance.Open(ButtonCombination.Yes, "CO-OP", "Invalid server data configuration, check IP or port", modalState: ModalState.Error, textYes: "Close");
        }

        public IEnumerator StartClientCoroutine()
        {
            try
            {
                _loadingClient = true;
                while (NetworkManager.Singleton.ShutdownInProgress) ;
                
                ModalManager.instance.Open(ButtonCombination.None, "CO-OP", "Connecting to the server...", modalState: ModalState.Info, textYes: "Close");
                NetworkManager.Singleton.GetComponent<UnityTransport>().MaxConnectAttempts = 10;
                NetworkingContainer.IsAllowJoins = true;
                var result = NetworkManager.Singleton.StartClient();
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
            
            var reason = NetworkManager.Singleton.DisconnectReason;
            ModalManager.instance.Open(ButtonCombination.Yes, "CO-OP",
                !string.IsNullOrWhiteSpace(reason) ? reason : "Failed to established connection with the remote server.",
                modalState: ModalState.Error, textYes: "Close");
            _loadingClient = false;
            if (!NetworkManager.Singleton.ShutdownInProgress)
                NetworkManager.Singleton.Shutdown();
        }

        private void SingletonOnOnClientConnectedCallback(ulong obj)
        {
            _loadingClient = false;
            NetworkManager.Singleton.GetComponent<UnityTransport>().MaxConnectAttempts = 60;
        }

        public bool IsConnectionDataValid()
        {
            return IPAddress.TryParse(NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address, out _);
        }

        public void SetNetworkIp(string ip)
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address = IPAddress.TryParse(ip, out _) ? ip : "127.0.0.1";
        }

        public void SetNetworkPort(string port)
        { 
            NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Port = ushort.TryParse(port, out var result) ? result : (ushort)7777;
        }
        
        public void StartServer()
        {
            StartCoroutine(StartServerCoroutine(GameData.GetCurrentStage().id));
        }

        private IEnumerator StartServerCoroutine(StageEnum currentStage)
        {
            try
            {
                if (SaveFile.Instance.IsSteamCoop())
                    SteamManager.instance.OpenLobby();
                
                TwitchIntegrationManager.instance.Connect();
                ModalManager.instance.Open(ButtonCombination.None, "Loading", "Loading the level...", modalState: ModalState.Info);
                if (!NetworkManager.Singleton.ShutdownInProgress)
                    NetworkManager.Singleton.Shutdown();

                while (NetworkManager.Singleton.ShutdownInProgress);

                NetworkingContainer.IsHostPlayer = true;
                AudioManager.instance.PlayButtonConfirmClick();
                SceneManager.LoadScene(currentStage.GetStringValue(), LoadSceneMode.Single);
                SceneManager.LoadScene("Scenes/Essential", LoadSceneMode.Additive);
			
                yield break;
            }
            catch (Exception e)
            {
                ModalManager.instance.Open(ButtonCombination.Yes, "Loading", $"Failed to load the game: {e.Message}", modalState: ModalState.Error, textYes: "Close");
            }
        }
    }
}