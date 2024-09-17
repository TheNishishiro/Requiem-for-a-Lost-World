using System;
using System.Collections.Generic;
using Interfaces;
using Objects;
using Objects.Items;
using Objects.Stage;
using Unity.Netcode;
using UnityEngine;

namespace UI.Labels.InGame.MP_List
{
    public class MpActivePlayersInGameList : MonoBehaviour
    {
        public static MpActivePlayersInGameList instance;
        [SerializeField] private MpCharacterInfoEntry characterInfoEntryPrefab;
        private Dictionary<ulong, MpCharacterInfoEntry> activeList = new ();
        
        public void Awake()
        {
            if (instance == null)
                instance = this;
        }

        public void UpdateEntryAvatar(ulong clientId, Sprite characterAvatar)
        {
            if (clientId == NetworkManager.Singleton.LocalClientId) return;
            
            if (!activeList.ContainsKey(clientId))
            {
                var newEntry = Instantiate(characterInfoEntryPrefab, transform);
                activeList.Add(clientId, newEntry);
            }

            activeList[clientId].SetAvatar(characterAvatar);
        }

        public void UpdateEntry(ulong clientId, float currentHp, float maxHp, string playerName, int level)
        {
            if (clientId == NetworkManager.Singleton.LocalClientId) return;
            
            if (!activeList.ContainsKey(clientId))
            {
                var newEntry = Instantiate(characterInfoEntryPrefab, transform);
                activeList.Add(clientId, newEntry);
            }
            
            activeList[clientId].SetData(currentHp, maxHp, level, string.IsNullOrWhiteSpace(playerName) ? clientId.ToString() : playerName);
        }
        
        public void RemoveEntry(ulong clientId)
        {
            if (activeList.ContainsKey(clientId))
            {
                Destroy(activeList[clientId].gameObject);
                activeList.Remove(clientId);
            }
        }

        public void UpdatePlayerItems(ulong clientId, ItemEnum itemId, WeaponEnum weaponId)
        {
            if (clientId == NetworkManager.Singleton.LocalClientId) 
                return;
            if (!activeList.ContainsKey(clientId))
                UpdateEntryAvatar(clientId, GameData.GetPlayerCharacterAvatar());
            
            var isWeapon = weaponId != WeaponEnum.Unset;
            var isItem = itemId != ItemEnum.Unset;
            
            if (!isWeapon && !isItem)
                return;

            IPlayerItem playerItem;
            if (isWeapon)
            {
                playerItem = WeaponManager.instance.GetWeapon(weaponId);
            }
            else
            {
                playerItem = WeaponManager.instance.GetItem(itemId);
            }
            
            activeList[clientId].AddOrUpdateItem(playerItem, isItem, isWeapon);
        }
    }
}