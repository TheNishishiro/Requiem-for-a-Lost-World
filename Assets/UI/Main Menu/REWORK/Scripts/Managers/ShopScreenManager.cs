using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Data;
using Interfaces;
using Managers;
using NaughtyAttributes;
using UnityEngine;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class ShopScreenManager : MonoBehaviour, IStackableWindow
    {
        [Space]
        [BoxGroup("Prefabs")] [SerializeField] private UpgradeEntry upgradeEntryPrefab;
        [Space]
        [BoxGroup("Container")] [SerializeField] private Transform containerUpgrade;
        private readonly List<UpgradeEntry> _upgradeEntries = new ();

        public void Update()
        {
            if (!IsInFocus) return;
            
            if (Input.GetKeyDown(KeyCode.Escape))
                Close();
        }

        public void Open()
        {
            if (!_upgradeEntries.Any())
            {
                var permUpgrades = PermUpgradeListManager.instance.GetUpgrades();
                foreach (var permUpgrade in permUpgrades)
                {
                    var upgradePanel = Instantiate(upgradeEntryPrefab, containerUpgrade);
                    _upgradeEntries.Add(upgradePanel);

                    upgradePanel.Setup(permUpgrade);
                }
            }
            else
            {
                _upgradeEntries.ForEach(x => x.Refresh());
            }

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