using System;
using System.Threading.Tasks;
using Netcode.Transports.Facepunch;
using Steamworks;
using Steamworks.Data;
using UI.Main_Menu.REWORK.Scripts;
using Unity.Netcode;
using UnityEngine;

namespace DefaultNamespace.Steam
{
    public class SteamManager : MonoBehaviour
    {
        public static SteamManager instance;
        [SerializeField] private uint _appId;
        private Lobby? _lobby;
        private FacepunchTransport transport = null;
        public Texture2D SteamAvatar;
        
        private async void Start()
        {
            if (instance == null)
                instance = this;

            DontDestroyOnLoad(gameObject);
            Debug.Log("SteamManager::Awake");
            try
            {
                transport = NetworkManager.Singleton.GetComponent<FacepunchTransport>();
                Debug.Log("SteamManager::Awake::try");
                if (!SteamClient.IsValid)
                    Steamworks.SteamClient.Init(_appId);
                var a = new Achievement("fdg");
                a.Trigger();
                
                SteamFriends.OnGameLobbyJoinRequested += OnGameLobbyJoinRequested;
                SteamMatchmaking.OnLobbyCreated += SteamMatchmaking_OnLobbyCreated;
                SteamMatchmaking.OnLobbyEntered += SteamMatchmaking_OnLobbyEntered;
                SteamMatchmaking.OnLobbyMemberJoined += SteamMatchmaking_OnLobbyMemberJoined;
                SteamMatchmaking.OnLobbyMemberLeave += SteamMatchmaking_OnLobbyMemberLeave;
                SteamMatchmaking.OnLobbyInvite += SteamMatchmaking_OnLobbyInvite;
                SteamMatchmaking.OnLobbyGameCreated += SteamMatchmaking_OnLobbyGameCreated;

                SteamAvatar = (await GetAvatar())?.CovertSteamAvatar();
            }
            catch (Exception e )
            {
                Debug.LogWarning(e);
            }
        }

        #region Multiplayer Callbacks
        // SERVER
        private void SteamMatchmaking_OnLobbyGameCreated(Lobby arg1, uint arg2, ushort arg3, SteamId arg4)
        {
            Debug.Log("SteamMatchmaking_OnLobbyGameCreated");
        }

        // -----
        private void SteamMatchmaking_OnLobbyInvite(Friend arg1, Lobby arg2)
        {
            Debug.Log("SteamMatchmaking_OnLobbyInvite");
        }

        // SERVER
        private void SteamMatchmaking_OnLobbyMemberLeave(Lobby arg1, Friend arg2)
        {
            Debug.Log("SteamMatchmaking_OnLobbyMemberLeave");
        }

        // SERVER
        private void SteamMatchmaking_OnLobbyMemberJoined(Lobby arg1, Friend arg2)
        {
            Debug.Log("SteamMatchmaking_OnLobbyMemberJoined");
        }

        // CLIENT
        private void SteamMatchmaking_OnLobbyEntered(Lobby obj)
        {
            Debug.Log("SteamMatchmaking_OnLobbyEntered");
        }

        // SERVER
        private void SteamMatchmaking_OnLobbyCreated(Result arg1, Lobby arg2)
        {
            Debug.Log("SteamMatchmaking_OnLobbyCreated");
            _lobby.Value.SetPublic();
            _lobby.Value.SetJoinable(true);
            _lobby.Value.SetGameServer(_lobby.Value.Owner.Id);
        }

        // CLIENT
        private async void OnGameLobbyJoinRequested(Lobby lobby, SteamId steamId)
        {
            Debug.Log($"OnGameLobbyJoinRequested: {steamId.Value}, {steamId.AccountId}");
            transport.targetSteamId = steamId;
            var joinedLobby = await lobby.Join();
            if(joinedLobby != RoomEnter.Success)
            {
                Debug.Log("Failed to create lobby");
            }
            else
            {
                _lobby = lobby;
                NetworkManager.Singleton.GetComponent<FacepunchTransport>().targetSteamId = steamId;
                NotificationManager.instance.DisplaySteamNotification($"Joined {lobby.Owner.Name}'s lobby.");
                Debug.Log("Joined Lobby");
            }
        }
        #endregion

        public async void OpenLobby()
        {
            if (!SteamClient.IsValid)
                return;
            _lobby = await SteamMatchmaking.CreateLobbyAsync(8);
        }

        public bool IsJoinedLobby()
        {
            return _lobby != null && !_lobby.Value.IsOwnedBy(SteamClient.SteamId);
        }
        private async Task<Image?> GetAvatar()
        {
            try
            {
                return await SteamFriends.GetLargeAvatarAsync( SteamClient.SteamId );
            }
            catch ( Exception e )
            {
                Debug.Log( e );
                return null;
            }
        }
        
        private void OnApplicationQuit()
        {
            SteamMatchmaking.OnLobbyCreated -= SteamMatchmaking_OnLobbyCreated;
            SteamMatchmaking.OnLobbyEntered -= SteamMatchmaking_OnLobbyEntered;
            SteamMatchmaking.OnLobbyMemberJoined -= SteamMatchmaking_OnLobbyMemberJoined;
            SteamMatchmaking.OnLobbyMemberLeave -= SteamMatchmaking_OnLobbyMemberLeave;
            SteamMatchmaking.OnLobbyInvite -= SteamMatchmaking_OnLobbyInvite;
            SteamMatchmaking.OnLobbyGameCreated -= SteamMatchmaking_OnLobbyGameCreated;
            SteamFriends.OnGameLobbyJoinRequested -= OnGameLobbyJoinRequested;
            
            if (_lobby != null)
                _lobby.Value.Leave();
            Debug.LogWarning("Steamworks shut down");
            OnDestroy();
        }

        private void OnDestroy()
        {
            SteamFriends.OnGameLobbyJoinRequested -= OnGameLobbyJoinRequested;
            if (SteamClient.IsValid)
                SteamClient.Shutdown();
            Destroy(gameObject);
        }
    }
}