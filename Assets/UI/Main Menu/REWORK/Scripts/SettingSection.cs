using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class SettingSection : MonoBehaviour, IPointerEnterHandler
    {
        [SerializeField] private Image background;
        [SerializeField] private Button button;
        [SerializeField] private Color enabledColor;
        [SerializeField] private Color disabledColor;
        [SerializeField] private SettingsSubSectionContainer subContainer;
        [SerializeField] private SettingsContainer settingsContainer;
        private SettingsScreenManager _settingsScreenManager;
        
        private void Start()
        {
            _settingsScreenManager = GetComponentInParent<SettingsScreenManager>();
            if (subContainer == null)
                subContainer =  GetComponentInParent<SettingsSubSectionContainer>(true);
        }

        public void OnClick()
        {
            if (!settingsContainer)
            {
                _settingsScreenManager.OpenSubSectionPanel();
                subContainer.Activate();
            }
            else if (settingsContainer)
            {
                _settingsScreenManager.OpenSettingsPanel();
                
                foreach (var container in FindObjectsByType<SettingsContainer>(FindObjectsSortMode.None))
                    container.Close();
                
                settingsContainer.Activate(subContainer);
                subContainer.DeactivateInput();
            }
        }
        
        public void Activate()
        {
            OnClick();
        }
        
        public void SetActive(bool isActive)
        {
            background.color = isActive ? enabledColor : disabledColor;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (settingsContainer)
                subContainer.UpdateSelection(GetInstanceID());
            else
                _settingsScreenManager.UpdateSelection(GetInstanceID());
        }
    }
}