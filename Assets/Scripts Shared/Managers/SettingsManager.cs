using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Data;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Slider = UnityEngine.UI.Slider;
using Toggle = UnityEngine.UI.Toggle;

namespace Managers
{
	public class SettingsManager : MonoBehaviour
	{
		[SerializeField] private SaveManager saveManager;
		[SerializeField] private Toggle vsyncToggle;
		[SerializeField] private Toggle discordToggle;
		[SerializeField] private Toggle use3DGrassToggle;
		[SerializeField] private Toggle displayCoopProjectilesToggle;
		[SerializeField] private TMP_Dropdown grassRenderDistanceDropdown;
		[SerializeField] private TMP_Dropdown grassDensityDropdown;
		[SerializeField] private TMP_Dropdown objectDensityDropdown;
		[SerializeField] private TMP_Dropdown qualityDropdown;
		[SerializeField] private TMP_Dropdown renderScalingDropdown;
		[SerializeField] private TMP_Dropdown shadowQualityDropdown;
		[SerializeField] private TMP_Dropdown antialiasingDropdown;
		[SerializeField] private TMP_Dropdown textureResolutionDropdown;
		[SerializeField] private TMP_Dropdown lodDropdown;
		[SerializeField] private TMP_Dropdown renderDistanceDropdown;
		[SerializeField] private TMP_Dropdown presetDropdown;
		[SerializeField] private TMP_Dropdown windowModeDropdown;
		[SerializeField] private TMP_Dropdown resolutionDropdown;
		[SerializeField] private TMP_InputField usernameField;
		[SerializeField] private Slider volumeSlider;
		private List<Resolution> availableResolutions = new ();
		private bool _isLoading;
		private const int minResolutionWidth = 800;
		private const int minResolutionHeight = 600;
		
		public void Start()
		{
			QualitySettings.vSyncCount = vsyncToggle.isOn ? 1 : 0;
		}
		
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
				Close();
		}

		public void ApplyPreset()
		{
			if (_isLoading) return;
			
			switch (presetDropdown.value)
			{
				case 0:
					vsyncToggle.isOn = false;
					grassRenderDistanceDropdown.value = 0;
					grassDensityDropdown.value = 0;
					qualityDropdown.value = 0;
					renderScalingDropdown.value = 2;
					shadowQualityDropdown.value = 0;
					antialiasingDropdown.value = 0;
					lodDropdown.value = 0;
					renderDistanceDropdown.value = 0;
					use3DGrassToggle.isOn = false;
					objectDensityDropdown.value = 0;
					textureResolutionDropdown.value = 2;
					break;
				case 1:
					vsyncToggle.isOn = false;
					grassRenderDistanceDropdown.value = 1;
					grassDensityDropdown.value = 1;
					qualityDropdown.value = 1;
					renderScalingDropdown.value = 2;
					shadowQualityDropdown.value = 0;
					antialiasingDropdown.value = 0;
					lodDropdown.value = 1;
					renderDistanceDropdown.value = 0;
					use3DGrassToggle.isOn = false;
					objectDensityDropdown.value = 0;
					textureResolutionDropdown.value = 2;
					break;
				case 2:
					vsyncToggle.isOn = true;
					grassRenderDistanceDropdown.value = 2;
					grassDensityDropdown.value = 2;
					qualityDropdown.value = 2;
					renderScalingDropdown.value = 4;
					shadowQualityDropdown.value = 1;
					antialiasingDropdown.value = 1;
					lodDropdown.value = 1;
					use3DGrassToggle.isOn = false;
					objectDensityDropdown.value = 1;
					textureResolutionDropdown.value = 3;
					break;
				case 3:
					vsyncToggle.isOn = true;
					grassRenderDistanceDropdown.value = 2;
					grassDensityDropdown.value = 3;
					qualityDropdown.value = 3;
					renderScalingDropdown.value = 4;
					shadowQualityDropdown.value = 3;
					antialiasingDropdown.value = 2;
					lodDropdown.value = 3;
					renderDistanceDropdown.value = 1;
					use3DGrassToggle.isOn = false;
					objectDensityDropdown.value = 1;
					textureResolutionDropdown.value = 4;
					break;
				case 4:
					vsyncToggle.isOn = true;
					grassRenderDistanceDropdown.value = 3;
					grassDensityDropdown.value = 3;
					qualityDropdown.value = 4;
					renderScalingDropdown.value = 4;
					shadowQualityDropdown.value = 4;
					antialiasingDropdown.value = 3;
					lodDropdown.value = 3;
					renderDistanceDropdown.value = 2;
					use3DGrassToggle.isOn = true;
					objectDensityDropdown.value = 2;
					textureResolutionDropdown.value = 4;
					break;
				case 5:
					vsyncToggle.isOn = true;
					grassRenderDistanceDropdown.value = 4;
					grassDensityDropdown.value = 3;
					qualityDropdown.value = 4;
					renderScalingDropdown.value = 4;
					shadowQualityDropdown.value = 5;
					antialiasingDropdown.value = 3;
					lodDropdown.value = 4;
					renderDistanceDropdown.value = 3;
					use3DGrassToggle.isOn = true;
					objectDensityDropdown.value = 3;
					textureResolutionDropdown.value = 4;
					break;
			}
		}

		public void Open()
		{
			_isLoading = true;
			var configuration = FindObjectOfType<SaveFile>().ConfigurationFile;
			presetDropdown.value = configuration.PresetIndex;
			vsyncToggle.isOn = configuration.Vsync;
			grassRenderDistanceDropdown.value = configuration.GrassRenderDistance;
			grassDensityDropdown.value = configuration.GrassDensity;
			objectDensityDropdown.value = configuration.ObjectDensity;
			qualityDropdown.value = configuration.Quality;
			renderScalingDropdown.value = configuration.RenderScaling;
			shadowQualityDropdown.value = configuration.ShadowQuality;
			antialiasingDropdown.value = configuration.AntiAliasing;
			lodDropdown.value = configuration.LodLevel;
			renderDistanceDropdown.value = configuration.RenderDistance;
			discordToggle.isOn = configuration.IsDiscordEnabled;
			windowModeDropdown.value = configuration.WindowMode;
			volumeSlider.value = configuration.Volume;
			textureResolutionDropdown.value = configuration.TextureQuality;
			usernameField.text = configuration.Username;
			displayCoopProjectilesToggle.isOn = configuration.RenderCoopProjectiles;
			
			resolutionDropdown.options ??= new List<TMP_Dropdown.OptionData>();
			resolutionDropdown.options.Clear();
			foreach (var resolution in Screen.resolutions)
			{
				if (resolution.width < minResolutionWidth || resolution.height < minResolutionHeight) 
					continue;
				
				availableResolutions.Add(resolution);
				resolutionDropdown.options.Add(new TMP_Dropdown.OptionData($"{resolution.width}x{resolution.height} @{resolution.refreshRateRatio.value:0}hz"));
			}
			resolutionDropdown.value = GetResolutionIndex(configuration.ResolutionWidth, configuration.ResolutionHeight, configuration.RefreshRate);
			
			gameObject.SetActive(true);
			_isLoading = false;
		}

		public void Close()
		{
			gameObject.SetActive(false);
		}
		
		public void SaveSettings()
		{
			var configuration = FindFirstObjectByType<SaveFile>().ConfigurationFile;
			configuration.PresetIndex = presetDropdown.value;
			configuration.Vsync = vsyncToggle.isOn;
			configuration.GrassRenderDistance = grassRenderDistanceDropdown.value;
			configuration.GrassDensity = grassDensityDropdown.value;
			configuration.ObjectDensity = objectDensityDropdown.value;
			configuration.Quality = qualityDropdown.value;
			configuration.RenderScaling = renderScalingDropdown.value;
			configuration.ShadowQuality = shadowQualityDropdown.value;
			configuration.AntiAliasing = antialiasingDropdown.value;
			configuration.LodLevel = lodDropdown.value;
			configuration.RenderDistance = renderDistanceDropdown.value;
			configuration.IsDiscordEnabled = discordToggle.isOn;
			configuration.WindowMode = windowModeDropdown.value;
			configuration.ResolutionWidth = availableResolutions[resolutionDropdown.value].width;
			configuration.ResolutionHeight = availableResolutions[resolutionDropdown.value].height;
			configuration.RefreshRate = availableResolutions[resolutionDropdown.value].refreshRateRatio.numerator;
			configuration.Volume = volumeSlider.value;
			configuration.TextureQuality = textureResolutionDropdown.value;
			configuration.Username = usernameField.text;
			configuration.RenderCoopProjectiles = displayCoopProjectilesToggle.isOn;
			
			saveManager.SaveGame();
			saveManager.ApplySettings();
			Close();
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

		public void LiveAdjustVolume()
		{
			AudioListener.volume = volumeSlider.value;
		}
	}
}