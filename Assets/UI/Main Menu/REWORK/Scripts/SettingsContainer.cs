using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class SettingsContainer : MonoBehaviour
    {
        private List<SettingsEntry> settingsEntries;
        private SettingsSubSectionContainer _relatedSubContainer;
        private int _selectionIndex;
        private const float KeyHoldDelay = 0.5f;
        private float _keyNextActionTime = 0f;

        private void Start()
        {
            settingsEntries = GetComponentsInChildren<SettingsEntry>(true).ToList();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Close();
            
            if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) 
                _keyNextActionTime = 0;
            
            if (Input.GetKey(KeyCode.UpArrow) && Time.time > _keyNextActionTime)
            {
                _keyNextActionTime = Time.time + KeyHoldDelay;
                _selectionIndex--;
                if (_selectionIndex < 0) _selectionIndex = settingsEntries.Count - 1;
                UpdateSelection(settingsEntries[_selectionIndex].GetInstanceID());
            }
            else if (Input.GetKey(KeyCode.DownArrow) && Time.time > _keyNextActionTime)
            {
                _keyNextActionTime = Time.time + KeyHoldDelay;
                _selectionIndex++;
                if (_selectionIndex >= settingsEntries.Count) _selectionIndex = 0;
                UpdateSelection(settingsEntries[_selectionIndex].GetInstanceID());
            }
            else if (Input.GetKey(KeyCode.LeftArrow) && Time.time > _keyNextActionTime)
            {
                _keyNextActionTime = Time.time + KeyHoldDelay;
                settingsEntries[_selectionIndex].Previous();
            }
            else if (Input.GetKey(KeyCode.RightArrow) && Time.time > _keyNextActionTime)
            {
                _keyNextActionTime = Time.time + KeyHoldDelay;
                settingsEntries[_selectionIndex].Next();
            }
        }

        public void UpdateSelection(int instanceId)
        {
            settingsEntries.ForEach(x => x.SetActive(false));
            _selectionIndex = settingsEntries.FindIndex(x => x.GetInstanceID() == instanceId);
            settingsEntries[_selectionIndex].SetActive(true);
        }
        
        public void Activate(SettingsSubSectionContainer subContainer)
        {
            _selectionIndex = 0;
            _relatedSubContainer = subContainer;
            gameObject.SetActive(true);
        }
        
        public void Close()
        {
            gameObject.SetActive(false);
            if (_relatedSubContainer != null)
                _relatedSubContainer.ActivateInput();
        }
    }
}