using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Shop;
using Interfaces;
using Managers;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class ShopScreenManager : MonoBehaviour, IStackableWindow
    {
        [BoxGroup("Buttons")] [SerializeField] private List<Button> buttonSections;
        [Space]
        [BoxGroup("Prefabs")] [SerializeField] private UpgradeEntry upgradeEntryPrefab;
        [Space]
        [BoxGroup("Container")] [SerializeField] private Transform containerUpgrade;
        [Space]
        [BoxGroup("Settings Entries")] [SerializeField] private GameObject containerUpgrades;
        [BoxGroup("Settings Entries")] [SerializeField] private GameObject containerExchange;
        [Space]
        [BoxGroup("Styling")] [SerializeField] private Material materialSelectedText;
        [BoxGroup("Styling")] [SerializeField] private Material materialIdleText;
        [BoxGroup("Styling")] [SerializeField] private Color colorHighlight;
        [Space]
        [BoxGroup("Labels")] [SerializeField] private TextMeshProUGUI textGold;
        [BoxGroup("Labels")] [SerializeField] private TextMeshProUGUI textGems;
        
        private readonly List<UpgradeEntry> _upgradeEntries = new ();

        private int _currentSectionId;
        private const float KeyHoldDelay = 0.25f;
        private float _keyNextActionTime = 0f;
        private bool _isSubsectionOpened;
        
        public void Update()
        {
            if (!IsInFocus) return;

            textGold.text = SaveFile.Instance.Gold.ToString();
            textGems.text = SaveFile.Instance.Gems.ToString();
            
            if (Input.GetKeyDown(KeyCode.Escape))
                Close();
            
            if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow)) 
                _keyNextActionTime = 0;
            
            if (Input.GetKey(KeyCode.UpArrow) && Time.time > _keyNextActionTime)
            {
                _keyNextActionTime = Time.time + KeyHoldDelay;
                _currentSectionId--;
                if (_currentSectionId < 0) _currentSectionId = buttonSections.Count - 1;
                FilterSection(_currentSectionId);
            }
            else if (Input.GetKey(KeyCode.DownArrow) && Time.time > _keyNextActionTime)
            {
                _keyNextActionTime = Time.time + KeyHoldDelay;
                _currentSectionId++;
                if (_currentSectionId >= buttonSections.Count) _currentSectionId = 0;
                FilterSection(_currentSectionId);
            }   
        }

        public void FilterSection(int sectionId)
        {
            _currentSectionId = sectionId;
            var section = (ShopSection)sectionId;
            containerUpgrades.SetActive(section is ShopSection.Upgrades);
            containerExchange.SetActive(section is ShopSection.Exchange);
            
            buttonSections.ForEach(x => x.GetComponent<Image>().color = Color.clear);
            buttonSections.ForEach(x => x.GetComponentInChildren<TextMeshProUGUI>().fontSharedMaterial = materialIdleText);
            buttonSections[sectionId].GetComponent<Image>().color = colorHighlight;
            buttonSections[sectionId].GetComponentInChildren<TextMeshProUGUI>().fontSharedMaterial = materialSelectedText;
        }

        public void Open()
        {
            FilterSection(0);
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