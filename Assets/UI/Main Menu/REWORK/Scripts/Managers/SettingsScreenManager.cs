using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Data;
using Interfaces;
using Managers;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class SettingsScreenManager : MonoBehaviour, IStackableWindow
    {
        [BoxGroup("UI Panels")] [SerializeField] private GameObject panelSection;
        [BoxGroup("UI Panels")] [SerializeField] private GameObject panelSubSection;
        [BoxGroup("UI Panels")] [SerializeField] private GameObject panelSettings;
        [Space]
        [BoxGroup("Settings Containers")] [SerializeField] private GameObject containerScreenSettings;
        [Space]
        [BoxGroup("Labels")] [SerializeField] private TextMeshProUGUI labelSubSectionPanelTitle;
        [BoxGroup("Labels")] [SerializeField] private TextMeshProUGUI labelSubSectionContainerTitle;
        [Space]
        [BoxGroup("Settings Entries")] [SerializeField] private SettingsEntry entryResolution;
        [BoxGroup("Settings Entries")] [SerializeField] private SettingsEntry entryWindowMode;
        [BoxGroup("Settings Entries")] [SerializeField] private SettingsEntry entryVSync;
        [BoxGroup("Settings Entries")] [SerializeField] private SettingsEntry entryRenderScaling;
        [BoxGroup("Settings Entries")] [SerializeField] private SettingsEntry entryVfxQuality;
        [BoxGroup("Settings Entries")] [SerializeField] private SettingsEntry entryTextureQuality;
        [BoxGroup("Settings Entries")] [SerializeField] private SettingsEntry entryLod;
        [BoxGroup("Settings Entries")] [SerializeField] private SettingsEntry entryRenderDistance;
        [BoxGroup("Settings Entries")] [SerializeField] private SettingsEntry entryShadows;
        [BoxGroup("Settings Entries")] [SerializeField] private SettingsEntry entryAntialiasing;
        [BoxGroup("Settings Entries")] [SerializeField] private SettingsEntry entry3dGrass;
        [BoxGroup("Settings Entries")] [SerializeField] private SettingsEntry entryGrassDensity;
        [BoxGroup("Settings Entries")] [SerializeField] private SettingsEntry entryGrassRenderDistance;
        [BoxGroup("Settings Entries")] [SerializeField] private SettingsEntry entryDiscordPresence;
        [BoxGroup("Settings Entries")] [SerializeField] private SettingsEntry entryCoopNickname;
        [BoxGroup("Settings Entries")] [SerializeField] private SettingsEntry entryCoopDisplayProjectiles;
        [BoxGroup("Settings Entries")] [SerializeField] private SettingsEntry entryVolume;
        [Space]
        [BoxGroup("Entries")] [SerializeField] private List<SettingSection> mainSettingSections;
        [Space]
        [BoxGroup("Animations")] [SerializeField] private Animator animator;
        
        private List<Resolution> availableResolutions = new ();
        private const int minResolutionWidth = 800;
        private const int minResolutionHeight = 600;
        
        private SaveFile _saveFile;
        private int _selectionIndex;
        private const float KeyHoldDelay = 0.2f;
        private float _keyNextActionTime = 0f;
        private bool _isSubsectionOpened;
        
        private void Update()
        {
            if (!IsInFocus || _isSubsectionOpened) return;
            
            if (Input.GetKeyDown(KeyCode.Escape))
                Close();
            
            if (Input.GetKeyDown(KeyCode.Return))
                mainSettingSections[_selectionIndex].Activate();
            
            if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow)) 
                _keyNextActionTime = 0;
            
            if (Input.GetKey(KeyCode.UpArrow) && Time.time > _keyNextActionTime)
            {
                _keyNextActionTime = Time.time + KeyHoldDelay;
                _selectionIndex--;
                if (_selectionIndex < 0) _selectionIndex = mainSettingSections.Count - 1;
                UpdateSelection(mainSettingSections[_selectionIndex].GetInstanceID());
            }
            else if (Input.GetKey(KeyCode.DownArrow) && Time.time > _keyNextActionTime)
            {
                _keyNextActionTime = Time.time + KeyHoldDelay;
                _selectionIndex++;
                if (_selectionIndex >= mainSettingSections.Count) _selectionIndex = 0;
                UpdateSelection(mainSettingSections[_selectionIndex].GetInstanceID());
            }    
        }

        public void UpdateSelection(int instanceId)
        {
            mainSettingSections.ForEach(x => x.SetActive(false));
            _selectionIndex = mainSettingSections.FindIndex(x => x.GetInstanceID() == instanceId);
            mainSettingSections[_selectionIndex].SetActive(true);
        }

        public void Open()
        {
            _saveFile = SaveManager.instance.GetSaveFile();
            panelSubSection.SetActive(false);
            panelSettings.SetActive(false);
            foreach (var settingsContainer in GetComponentsInChildren<SettingsContainer>())
                settingsContainer.gameObject.SetActive(false);
            foreach (var subSectionContainer in GetComponentsInChildren<SettingsSubSectionContainer>())
                subSectionContainer.gameObject.SetActive(false);
            
            LoadSettings();
            StackableWindowManager.instance.OpenWindow(this);
        }

        public void Close()
        {
            SaveSettings();
            StackableWindowManager.instance.CloseWindow(this);
        }

        private void LoadSettings()
        {
            var configuration = _saveFile.ConfigurationFile;
            foreach (var resolution in Screen.resolutions)
            {
                if (resolution.width < minResolutionWidth || resolution.height < minResolutionHeight) 
                    continue;
				
                availableResolutions.Add(resolution);
            }
            entryResolution.SetOptions(availableResolutions.Select(x => $"{x.width}x{x.height} @{x.refreshRateRatio.value:0}hz").ToArray(), GetResolutionIndex(configuration.ResolutionWidth, configuration.ResolutionHeight, configuration.RefreshRate));
            entryWindowMode.SetSelection(configuration.WindowMode);
            entryVSync.SetSelection(configuration.Vsync ? 1 : 0);
            entryRenderScaling.SetSelection(configuration.RenderScaling);
            entryVfxQuality.SetSelection(configuration.Quality);
            entryTextureQuality.SetSelection(configuration.TextureQuality);
            entryLod.SetSelection(configuration.LodLevel);
            entryRenderDistance.SetSelection(configuration.RenderDistance);
            entryShadows.SetSelection(configuration.ShadowQuality);
            entryAntialiasing.SetSelection(configuration.AntiAliasing);
            entry3dGrass.SetSelection(configuration.Use3dGrass ? 1 : 0);
            entryGrassDensity.SetSelection(configuration.GrassDensity);
            entryGrassRenderDistance.SetSelection(configuration.GrassRenderDistance);
            entryDiscordPresence.SetSelection(configuration.IsDiscordEnabled ? 1 : 0);
            entryCoopNickname.SetText(configuration.Username);
            entryCoopDisplayProjectiles.SetSelection(configuration.RenderCoopProjectiles ? 1 : 0);
            entryVolume.SetSliderValue(configuration.Volume);
        }

        private void SaveSettings()
        {
            var configuration = _saveFile.ConfigurationFile;

            configuration.ResolutionWidth = availableResolutions[entryResolution.GetSelectedOption()].width;
            configuration.ResolutionHeight = availableResolutions[entryResolution.GetSelectedOption()].height;
            configuration.RefreshRate = availableResolutions[entryResolution.GetSelectedOption()].refreshRateRatio.numerator;
            configuration.WindowMode = entryWindowMode.GetSelectedOption();
            configuration.Vsync = entryVSync.GetSelectedOption() == 1;
            configuration.RenderScaling = entryRenderScaling.GetSelectedOption();
            configuration.Quality = entryVfxQuality.GetSelectedOption();
            configuration.TextureQuality = entryTextureQuality.GetSelectedOption();
            configuration.LodLevel = entryLod.GetSelectedOption();
            configuration.RenderDistance = entryRenderDistance.GetSelectedOption();
            configuration.ShadowQuality = entryShadows.GetSelectedOption();
            configuration.AntiAliasing = entryAntialiasing.GetSelectedOption();
            configuration.Use3dGrass = entry3dGrass.GetSelectedOption() == 1;
            configuration.GrassDensity = entryGrassDensity.GetSelectedOption();
            configuration.GrassRenderDistance = entryGrassRenderDistance.GetSelectedOption();
            configuration.IsDiscordEnabled = entryDiscordPresence.GetSelectedOption() == 1;
            configuration.RenderCoopProjectiles = entryCoopDisplayProjectiles.GetSelectedOption() == 1;
            configuration.Username = entryCoopNickname.GetText();
            configuration.Volume = entryVolume.GetSliderValue();
            
            SaveManager.instance.ApplySettings();
            SaveManager.instance.SaveGame();
        }
        
        private int GetResolutionIndex(int width, int height, uint refreshRate)
        {
            var bestMatchResolution = availableResolutions[0];
            var bestMatchIndex = -1;
            var hasMatchingResolution = false;

            for (var i = 0; i < availableResolutions.Count; i++)
            {
                var resolution = availableResolutions[i];
                if (resolution.width == width && resolution.height == height)
                {
                    hasMatchingResolution = true;
                    if (resolution.refreshRateRatio.numerator == refreshRate)
                    {
                        // Exact match found, return immediately
                        return i;
                    }
                    else if (bestMatchResolution.refreshRateRatio.numerator < resolution.refreshRateRatio.numerator)
                    {
                        // We found same resolution but with a better refresh rate, update our best match
                        bestMatchResolution = resolution;
                        bestMatchIndex = i;
                    }
                }
                else if (bestMatchResolution.width < resolution.width)
                {
                    // This is a higher resolution, update our fallback option
                    bestMatchResolution = resolution;
                    bestMatchIndex = i;
                }
            }

            // If we found a matching resolution with best available refresh rate, return that index
            // Otherwise return highest resolution available
            return hasMatchingResolution ? bestMatchIndex : availableResolutions.Count - 1;   
        }
        
        public bool IsInFocus { get; set; }
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void OpenSubSectionPanel()
        {
            _isSubsectionOpened = true;
            panelSubSection.SetActive(true);
        }

        public void CloseSubSectionPanel()
        {
            var sectionComponent = panelSubSection.GetComponentInChildren<SettingsSubSectionContainer>();
            if (sectionComponent != null)
                sectionComponent.gameObject.SetActive(false);
            if (panelSettings.activeSelf)
                CloseSettingsPanel();
                
            _isSubsectionOpened = false;
            panelSubSection.SetActive(false);
        }

        public void OpenSettingsPanel()
        {
            panelSettings.SetActive(true);
        }

        public void CloseSettingsPanel()
        {
            var settingsContainer = panelSettings.GetComponentInChildren<SettingsContainer>();
            if (settingsContainer != null)
                settingsContainer.Close();
            panelSettings.SetActive(false);
        }

        public void LiveAdjustVolume()
        {
            AudioListener.volume = entryVolume.GetSliderValue();
        }
    }
}