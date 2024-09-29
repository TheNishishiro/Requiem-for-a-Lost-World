using System;
using UI.Shared;
using UnityEngine;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class NotificationManager : MonoBehaviour
    {
        public static NotificationManager instance;
        [SerializeField] private FancyNotificationDisplay steamNotification;

        private void Start()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }

        public void DisplaySteamNotification(string text)
        {
            steamNotification.Display(text, "Steam");
        }
    }
}