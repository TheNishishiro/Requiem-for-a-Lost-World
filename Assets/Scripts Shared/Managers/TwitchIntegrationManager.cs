using System;
using System.Collections;
using DefaultNamespace.Data;
using Lexone.UnityTwitchChat;
using UnityEngine;

namespace Managers
{
    public class TwitchIntegrationManager : MonoBehaviour
    {
        public static TwitchIntegrationManager instance;
        private bool isConnected;
        
        private void Start()
        {
            if (instance == null)
            {
                instance = this;
            }
            
        }

        public void Connect()
        {
            if (!IsTwitchIntegrationEnabled()) 
                return;
            
            IRC.Instance.channel = SaveFile.Instance.ConfigurationFile.TwitchChannel;
            IRC.Instance.Connect();
            IRC.Instance.JoinChannel(IRC.Instance.channel);
            isConnected = true;
        }

        public bool IsTwitchIntegrationEnabled()
        {
            return SaveFile.Instance.ConfigurationFile.TwitchEnabled &&
                   !string.IsNullOrWhiteSpace(SaveFile.Instance.ConfigurationFile.TwitchChannel);
        }

        public bool IsEnabledAndConnected()
        {
            return IsTwitchIntegrationEnabled() && IsConnected();
        }

        public bool IsChatPicksItemsEnabled()
        {
            return IsEnabledAndConnected() && SaveFile.Instance.ConfigurationFile.TwitchPickItems;
        }

        public bool IsConnected()
        {
            return isConnected;
        }
    }
}