using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class SettingsSubSectionContainer : MonoBehaviour
    {
        private List<SettingSection> _sectionButtons;
        private SettingsScreenManager _settingsScreenManager;
        private int _selectedIndex;
        private bool _isSectionOpened;
        private const float KeyHoldDelay = 0.2f;
        private float _keyNextActionTime = 0f;

        private void Start()
        {
            _settingsScreenManager = GetComponentInParent<SettingsScreenManager>();
            _sectionButtons = GetComponentsInChildren<SettingSection>(true).ToList();
        }

        private void Update()
        {
            if (_isSectionOpened) return;
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gameObject.SetActive(false);
                _settingsScreenManager.CloseSubSectionPanel();
            }
            
            if (Input.GetKeyDown(KeyCode.Return))
                _sectionButtons[_selectedIndex].Activate();
            
            if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow)) 
                _keyNextActionTime = 0;
            
            if (Input.GetKey(KeyCode.UpArrow) && Time.time > _keyNextActionTime)
            {
                _keyNextActionTime = Time.time + KeyHoldDelay;
                _selectedIndex--;
                if (_selectedIndex < 0) _selectedIndex = _sectionButtons.Count - 1;
                UpdateSelection(_sectionButtons[_selectedIndex].GetInstanceID());
            }
            else if (Input.GetKey(KeyCode.DownArrow) && Time.time > _keyNextActionTime)
            {
                _keyNextActionTime = Time.time + KeyHoldDelay;
                _selectedIndex++;
                if (_selectedIndex >= _sectionButtons.Count) _selectedIndex = 0;
                UpdateSelection(_sectionButtons[_selectedIndex].GetInstanceID());
            }
        }

        public void UpdateSelection(int instanceId, bool isActive = true)
        {
            _sectionButtons.ForEach(x => x.SetActive(false));
            _selectedIndex = _sectionButtons.FindIndex(x => x.GetInstanceID() == instanceId);
            _sectionButtons[_selectedIndex].SetActive(isActive);
        }

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void ActivateInput()
        {
            _isSectionOpened = false;
        }

        public void DeactivateInput()
        {
            _isSectionOpened = true;
        }
    }
}