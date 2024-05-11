using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Environment;
using DefaultNamespace.Data.Settings;
using Interfaces;
using Managers;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class SettingsScreenManager : MonoBehaviour, IStackableWindow
    {
	    public static SettingsScreenManager instance;
        [BoxGroup("Buttons")] [SerializeField] private List<Button> buttonSections;
        [Space]
        [BoxGroup("Styling")] [SerializeField] private Material materialSelectedText;
        [BoxGroup("Styling")] [SerializeField] private Material materialIdleText;
        [BoxGroup("Styling")] [SerializeField] private Color colorHighlight;
        [Space]
        [BoxGroup("Section Containers")] [SerializeField] private GameObject containerGraphicSettings;
        [BoxGroup("Section Containers")] [SerializeField] private GameObject containerSoundSettings;
        [BoxGroup("Section Containers")] [SerializeField] private GameObject containerIntegrationSettings;
        [BoxGroup("Section Containers")] [SerializeField] private GameObject containerMultiplayerSettings;
        [BoxGroup("Section Containers")] [SerializeField] private GameObject containerControlsSettings;
        [Space]
        [BoxGroup("Settings Entries")] [SerializeField] private SettingsEntry entryPreset;
        [BoxGroup("Settings Entries")] [SerializeField] private SettingsEntry entryResolution;
        [BoxGroup("Settings Entries")] [SerializeField] private SettingsEntry entryWindowMode;
        [BoxGroup("Settings Entries")] [SerializeField] private SettingsEntry entryVSync;
        [BoxGroup("Settings Entries")] [SerializeField] private SettingsEntry entryMaxFps;
        [BoxGroup("Settings Entries")] [SerializeField] private SettingsEntry entryRenderScaling;
        [BoxGroup("Settings Entries")] [SerializeField] private SettingsEntry entryUpscaling;
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
        [BoxGroup("Settings Entries")] [SerializeField] private SettingsEntry entryAbilityKeyBind;
        [BoxGroup("Settings Entries")] [SerializeField] private SettingsEntry entryMoveUpKeyBind;
        [BoxGroup("Settings Entries")] [SerializeField] private SettingsEntry entryMoveDownKeyBind;
        [BoxGroup("Settings Entries")] [SerializeField] private SettingsEntry entryMoveLeftKeyBind;
        [BoxGroup("Settings Entries")] [SerializeField] private SettingsEntry entryMoveRightKeyBind;
        [BoxGroup("Settings Entries")] [SerializeField] private SettingsEntry entryDashKeyBind;
        [BoxGroup("Settings Entries")] [SerializeField] private SettingsEntry entrySprintKeyBind;
        [Space]
        [BoxGroup("Description")] [SerializeField] private Image imageExample;
        [BoxGroup("Description")] [SerializeField] private TextMeshProUGUI textDescription;
        
        
        private readonly List<Resolution> _availableResolutions = new ();
        private const int MinResolutionWidth = 800;
        private const int MinResolutionHeight = 600;
        
        private SaveFile _saveFile;
        private int _currentSectionId;
        private const float KeyHoldDelay = 0.25f;
        private float _keyNextActionTime = 0f;
        private bool _isSubsectionOpened;

        private void Start()
        {
	        if (instance == null)
		        instance = this;
        }

        private void Update()
        {
            if (!IsInFocus || _isSubsectionOpened) return;
            
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
            var section = (SettingsSection)sectionId;
            containerGraphicSettings.SetActive(section is SettingsSection.Graphics);
            containerSoundSettings.SetActive(section is SettingsSection.Sound);
            containerIntegrationSettings.SetActive(section is SettingsSection.Integration);
            containerMultiplayerSettings.SetActive(section is SettingsSection.Multiplayer);
            containerControlsSettings.SetActive(section is SettingsSection.Controls);
            
            buttonSections.ForEach(x => x.GetComponent<Image>().color = Color.clear);
            buttonSections.ForEach(x => x.GetComponentInChildren<TextMeshProUGUI>().fontSharedMaterial = materialIdleText);
            buttonSections[sectionId].GetComponent<Image>().color = colorHighlight;
            buttonSections[sectionId].GetComponentInChildren<TextMeshProUGUI>().fontSharedMaterial = materialSelectedText;
        }

        public void OpenDescription(string description, Sprite exampleImage)
        {
	        imageExample.sprite = exampleImage;
	        imageExample.gameObject.SetActive(exampleImage != null);

	        textDescription.text = description;
	        textDescription.gameObject.SetActive(!string.IsNullOrWhiteSpace(description));
        }
        
        public void Open()
        {
            _saveFile = SaveManager.instance.GetSaveFile();
            LoadSettings();
            FilterSection(0);
            OpenDescription(null, null);
            StackableWindowManager.instance.OpenWindow(this);
        }

        public void Close()
        {
            SaveSettings();
            StackableWindowManager.instance.CloseWindow(this);
        }

        public void SetKeyBind(int actionCode)
        {
	        var contextEntry = (KeyAction)actionCode switch
	        {
		        KeyAction.Ability => entryAbilityKeyBind,
		        KeyAction.MoveUp => entryMoveDownKeyBind,
		        KeyAction.MoveDown => entryMoveDownKeyBind,
		        KeyAction.MoveLeft => entryMoveLeftKeyBind,
		        KeyAction.MoveRight => entryMoveRightKeyBind,
		        KeyAction.Dash => entryDashKeyBind,
		        KeyAction.Sprint => entrySprintKeyBind,
		        _ => throw new ArgumentOutOfRangeException(nameof(actionCode), actionCode, null)
	        };
	        
	        StartCoroutine(WaitForPlayerInput(contextEntry, (KeyAction)actionCode));
        }

        private IEnumerator WaitForPlayerInput(SettingsEntry settingsEntry, KeyAction keyAction)
        {
	        var waitTime = 5f;
	        settingsEntry.SetLabelValue("Press any key");
	        while (waitTime > 0)
	        {
		        waitTime -= Time.deltaTime;
		        foreach(KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
		        {
			        if (Input.GetKeyDown(keyCode))
			        {
				        settingsEntry.SetLabelValue(keyCode.ToString(), (int)keyCode);
				        yield break;
			        }
		        }
		        yield return null;
	        }

	        var currentKeyBind = _saveFile.GetKeybinding(keyAction);
	        settingsEntry.SetLabelValue(currentKeyBind.ToString(), (int)currentKeyBind);
        }

        public void ApplyPreset()
        {
            switch (entryPreset.GetSelectedOption())
			{
				case 0:
					entryVSync.SetSelection(0);
					entryGrassRenderDistance.SetSelection(0);
					entryGrassDensity.SetSelection(0);
					entryVfxQuality.SetSelection(0);
					entryTextureQuality.SetSelection(0);
					entryRenderScaling.SetSelection(2);
					entryShadows.SetSelection(0);
					entryAntialiasing.SetSelection(0);
					entryLod.SetSelection(0);
					entryRenderDistance.SetSelection(0);
					entry3dGrass.SetSelection(0);
					break;
				case 1:
					entryVSync.SetSelection(0);
					entryGrassRenderDistance.SetSelection(1);
					entryGrassDensity.SetSelection(1);
					entryVfxQuality.SetSelection(1);
					entryTextureQuality.SetSelection(2);
					entryRenderScaling.SetSelection(2);
					entryShadows.SetSelection(0);
					entryAntialiasing.SetSelection(0);
					entryLod.SetSelection(1);
					entryRenderDistance.SetSelection(0);
					entry3dGrass.SetSelection(0);
					break;
				case 2:
					entryVSync.SetSelection(1);
					entryGrassRenderDistance.SetSelection(2);
					entryGrassDensity.SetSelection(2);
					entryVfxQuality.SetSelection(2);
					entryTextureQuality.SetSelection(4);
					entryRenderScaling.SetSelection(3);
					entryShadows.SetSelection(1);
					entryAntialiasing.SetSelection(1);
					entryLod.SetSelection(1);
					entryRenderDistance.SetSelection(1);
					entry3dGrass.SetSelection(0);
					break;
				case 3:
					entryVSync.SetSelection(1);
					entryGrassRenderDistance.SetSelection(2);
					entryGrassDensity.SetSelection(3);
					entryVfxQuality.SetSelection(3);
					entryTextureQuality.SetSelection(4);
					entryRenderScaling.SetSelection(4);
					entryShadows.SetSelection(3);
					entryAntialiasing.SetSelection(2);
					entryLod.SetSelection(3);
					entryRenderDistance.SetSelection(1);
					entry3dGrass.SetSelection(0);
					break;
				case 4:
					entryVSync.SetSelection(1);
					entryGrassRenderDistance.SetSelection(3);
					entryGrassDensity.SetSelection(3);
					entryVfxQuality.SetSelection(4);
					entryTextureQuality.SetSelection(4);
					entryRenderScaling.SetSelection(4);
					entryShadows.SetSelection(4);
					entryAntialiasing.SetSelection(3);
					entryLod.SetSelection(3);
					entryRenderDistance.SetSelection(2);
					entry3dGrass.SetSelection(1);
					break;
				case 5:
					entryVSync.SetSelection(1);
					entryGrassRenderDistance.SetSelection(4);
					entryGrassDensity.SetSelection(3);
					entryVfxQuality.SetSelection(4);
					entryTextureQuality.SetSelection(4);
					entryRenderScaling.SetSelection(4);
					entryShadows.SetSelection(5);
					entryAntialiasing.SetSelection(3);
					entryLod.SetSelection(4);
					entryRenderDistance.SetSelection(3);
					entry3dGrass.SetSelection(1);
					break;
			}
        }

        private void LoadSettings()
        {
            var configuration = _saveFile.ConfigurationFile;
            foreach (var resolution in Screen.resolutions)
            {
                if (resolution.width < MinResolutionWidth || resolution.height < MinResolutionHeight) 
                    continue;
				
                _availableResolutions.Add(resolution);
            }
            entryResolution.SetOptions(_availableResolutions.Select(x => $"{x.width}x{x.height} @{x.refreshRateRatio.value:0}hz").ToArray(), GetResolutionIndex(configuration.ResolutionWidth, configuration.ResolutionHeight, configuration.RefreshRate));
            entryWindowMode.SetSelection(configuration.WindowMode);
            entryVSync.SetSelection(configuration.Vsync ? 1 : 0);
            entryUpscaling.SetSelection(configuration.UpscalingMethod);
            entryMaxFps.SetSelection(configuration.FpsLimit);
            entryRenderScaling.SetSelection(configuration.RenderScaling);
            entryVfxQuality.SetSelection(configuration.Quality);
            entryTextureQuality.SetSelection(configuration.TextureQuality);
            entryLod.SetSelection(configuration.LodLevel);
            entryRenderDistance.SetSelection(configuration.RenderDistance);
            entryShadows.SetSelection(configuration.ShadowQuality);
            entryAntialiasing.SetSelection(configuration.AntiAliasing);
            entry3dGrass.SetSelection((int)configuration.GrassType);
            entryGrassDensity.SetSelection(configuration.GrassDensity);
            entryGrassRenderDistance.SetSelection(configuration.GrassRenderDistance);
            entryDiscordPresence.SetSelection(configuration.IsDiscordEnabled ? 1 : 0);
            entryCoopNickname.SetText(configuration.Username);
            entryCoopDisplayProjectiles.SetSelection(configuration.RenderCoopProjectiles ? 1 : 0);
            entryVolume.SetSliderValue(configuration.Volume);
            var abilityKeyBind = _saveFile.GetKeybinding(KeyAction.Ability);
            entryAbilityKeyBind.SetLabelValue(abilityKeyBind.ToString(), (int)abilityKeyBind);
            var moveUpKeyBind = _saveFile.GetKeybinding(KeyAction.MoveUp);
            entryMoveUpKeyBind.SetLabelValue(moveUpKeyBind.ToString(), (int)moveUpKeyBind);
            var moveDownKeyBind = _saveFile.GetKeybinding(KeyAction.MoveDown);
            entryMoveDownKeyBind.SetLabelValue(moveDownKeyBind.ToString(), (int)moveDownKeyBind);
            var moveLeftKeyBind = _saveFile.GetKeybinding(KeyAction.MoveLeft);
            entryMoveLeftKeyBind.SetLabelValue(moveLeftKeyBind.ToString(), (int)moveLeftKeyBind);
            var moveRightKeyBind = _saveFile.GetKeybinding(KeyAction.MoveRight);
            entryMoveRightKeyBind.SetLabelValue(moveRightKeyBind.ToString(), (int)moveRightKeyBind);
            var dashKeyBind = _saveFile.GetKeybinding(KeyAction.Dash);
            entryDashKeyBind.SetLabelValue(dashKeyBind.ToString(), (int)dashKeyBind);
            var sprintKeyBind = _saveFile.GetKeybinding(KeyAction.Sprint);
            entrySprintKeyBind.SetLabelValue(sprintKeyBind.ToString(), (int)sprintKeyBind);
        }

        private void SaveSettings()
        {
            var configuration = _saveFile.ConfigurationFile;

            configuration.ResolutionWidth = _availableResolutions[entryResolution.GetSelectedOption()].width;
            configuration.ResolutionHeight = _availableResolutions[entryResolution.GetSelectedOption()].height;
            configuration.RefreshRate = _availableResolutions[entryResolution.GetSelectedOption()].refreshRateRatio.numerator;
            configuration.WindowMode = entryWindowMode.GetSelectedOption();
            configuration.Vsync = entryVSync.GetSelectedOption() == 1;
            configuration.UpscalingMethod = entryUpscaling.GetSelectedOption();
            configuration.FpsLimit = entryMaxFps.GetSelectedOption();
            configuration.RenderScaling = entryRenderScaling.GetSelectedOption();
            configuration.Quality = entryVfxQuality.GetSelectedOption();
            configuration.TextureQuality = entryTextureQuality.GetSelectedOption();
            configuration.LodLevel = entryLod.GetSelectedOption();
            configuration.RenderDistance = entryRenderDistance.GetSelectedOption();
            configuration.ShadowQuality = entryShadows.GetSelectedOption();
            configuration.AntiAliasing = entryAntialiasing.GetSelectedOption();
            configuration.GrassType = (GrassType)entry3dGrass.GetSelectedOption();
            configuration.GrassDensity = entryGrassDensity.GetSelectedOption();
            configuration.GrassRenderDistance = entryGrassRenderDistance.GetSelectedOption();
            configuration.IsDiscordEnabled = entryDiscordPresence.GetSelectedOption() == 1;
            configuration.RenderCoopProjectiles = entryCoopDisplayProjectiles.GetSelectedOption() == 1;
            configuration.Username = entryCoopNickname.GetText();
            configuration.Volume = entryVolume.GetSliderValue();
            _saveFile.Keybindings[KeyAction.Ability] = (KeyCode)entryAbilityKeyBind.GetLabelValue();
            _saveFile.Keybindings[KeyAction.MoveUp] = (KeyCode)entryMoveUpKeyBind.GetLabelValue();
            _saveFile.Keybindings[KeyAction.MoveDown] = (KeyCode)entryMoveDownKeyBind.GetLabelValue();
            _saveFile.Keybindings[KeyAction.MoveLeft] = (KeyCode)entryMoveLeftKeyBind.GetLabelValue();
            _saveFile.Keybindings[KeyAction.MoveRight] = (KeyCode)entryMoveRightKeyBind.GetLabelValue();
            _saveFile.Keybindings[KeyAction.Dash] = (KeyCode)entryDashKeyBind.GetLabelValue();
            _saveFile.Keybindings[KeyAction.Sprint] = (KeyCode)entrySprintKeyBind.GetLabelValue();
            
            SaveManager.instance.ApplySettings();
            SaveManager.instance.SaveGame();
        }
        
        private int GetResolutionIndex(int width, int height, uint refreshRate)
        {
            var bestMatchResolution = _availableResolutions[0];
            var bestMatchIndex = -1;
            var hasMatchingResolution = false;

            for (var i = 0; i < _availableResolutions.Count; i++)
            {
                var resolution = _availableResolutions[i];
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
            return hasMatchingResolution ? bestMatchIndex : _availableResolutions.Count - 1;   
        }
        
        public bool IsInFocus { get; set; }
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void LiveAdjustVolume()
        {
            AudioListener.volume = entryVolume.GetSliderValue();
        }
    }
}