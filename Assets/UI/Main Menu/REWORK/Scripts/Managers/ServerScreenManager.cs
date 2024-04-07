using System;
using System.Collections.Generic;
using System.Net;
using DefaultNamespace.Data;
using Interfaces;
using Managers;
using NaughtyAttributes;
using Objects.Stage;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class ServerScreenManager : MonoBehaviour, IStackableWindow
    {
        public static ServerScreenManager instance;
        [Space]
        [BoxGroup("Managers")] [SerializeField] private MultiplayerManager multiplayerManager;
        [Space]
        [BoxGroup("Prefabs")] [SerializeField] private ServerEntry serverEntryPrefab;
        [Space]
        [BoxGroup("Containers")] [SerializeField] private Transform serverListContainer;
        [Space]
        [BoxGroup("Panels")] [SerializeField] private GameObject serverSettingsScreen;
        [BoxGroup("Panels")] [SerializeField] private GameObject serverListScreen;
        [Space]
        [BoxGroup("Inputs")] [SerializeField] private TMP_InputField inputServerName;
        [BoxGroup("Inputs")] [SerializeField] private TMP_InputField inputServerIp;
        [BoxGroup("Inputs")] [SerializeField] private TMP_InputField inputServerPort;
        [Space]
        [BoxGroup("Images")] [SerializeField] private Image imageCharacterCard;
        [Space]
        [BoxGroup("Labels")] [SerializeField] private TextMeshProUGUI labelAddEntry;
        [Space]
        [BoxGroup("Animator")] [SerializeField] private Animator animator;
        
        private List<ServerEntry> serverList = new ();
        private bool _isAddNew;
        private KeyValuePair<ServerEntry, ServerData> entryDataPair;
        
        private void Start()
        {
            if (instance == null)
                instance = this;
            
            foreach (var server in SaveFile.Instance.Servers)
            {
                var serverEntry = Instantiate(serverEntryPrefab, serverListContainer);
                serverEntry.Setup(server);
                serverList.Add(serverEntry);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                CloseWithAnimation();
            
            if (Input.GetKeyDown(KeyCode.Return) && serverSettingsScreen.activeSelf)
                AddServerToList();

            if (Input.GetKeyDown(KeyCode.Tab) && serverSettingsScreen.activeSelf)
            {
                if (inputServerName.isFocused)
                {
                    inputServerIp.Select();
                }
                else if (inputServerIp.isFocused)
                {
                    inputServerPort.Select();
                }
                else
                {
                    inputServerName.Select();
                }
            }
        }

        public void OpenAddServerScreen()
        {
            _isAddNew = true;
            labelAddEntry.text = "Add new";
            inputServerName.Select();
            inputServerName.text = null;
            inputServerIp.text = null;
            inputServerPort.text = null;
            animator.SetTrigger("SwitchToSettings");
        }

        public void AddServerToList()
        {
            int.TryParse(inputServerPort.text, out var port);
            if (_isAddNew)
            {
                var serverData = new ServerData()
                {
                    Name = inputServerName.text,
                    IpAddress = inputServerIp.text,
                    Port = port
                };

                var serverEntry = Instantiate(serverEntryPrefab, serverListContainer);
                serverEntry.Setup(serverData);
                serverList.Add(serverEntry);
                SaveFile.Instance.Servers.Add(serverData);
            }
            else
            {
                var serverData = entryDataPair.Value;
                serverData.Name = inputServerName.text;
                serverData.IpAddress = inputServerIp.text;
                serverData.Port = port;
                
                entryDataPair.Key.Setup(serverData);
            }
            
            SaveFile.Instance.Save();
            animator.SetTrigger("SwitchToList");
        }

        public void OpenEditEntry(ServerData serverData, ServerEntry serverEntry)
        {
            _isAddNew = false;
            labelAddEntry.text = "Save changes";
            entryDataPair = new KeyValuePair<ServerEntry, ServerData>(serverEntry, serverData);
            inputServerName.Select();
            inputServerName.text = serverData.Name;
            inputServerIp.text = serverData.IpAddress;
            inputServerPort.text = serverData.Port.ToString();
            animator.SetTrigger("SwitchToSettings");
        }

        public void Connect(ServerData serverData)
        {
            multiplayerManager.SetNetworkIp(serverData.IpAddress);
            multiplayerManager.SetNetworkPort(serverData.Port.ToString());
            multiplayerManager.StartClient();
        }

        public void DeleteServerEntry(ServerData serverData, int instanceID)
        {
            var entryToRemove = serverList.Find(entry => entry.GetInstanceID() == instanceID);
            if (entryToRemove != null)
            {
                serverList.Remove(entryToRemove);
                SaveFile.Instance.Servers.Remove(serverData);
                SaveFile.Instance.Save();
                Destroy(entryToRemove.gameObject);
            }
        }

        public void CloseWithAnimation()
        {
            animator.SetTrigger(serverSettingsScreen.activeSelf ? "SwitchToList" : "Close");
        }

        public void Open()
        {
            serverListScreen.SetActive(true);
            serverSettingsScreen.SetActive(false);
            var character = CharacterListManager.instance.GetActiveCharacter();
            imageCharacterCard.sprite = character.FullArt;
            
            StackableWindowManager.instance.OpenWindow(this);
        }

        public void Close()
        {
            StackableWindowManager.instance.CloseWindow(this);
        }
        
        public bool IsInFocus { get; set; }
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}