using System;
using DefaultNamespace.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
	public class SettingsManager : MonoBehaviour
	{
		[SerializeField] private SaveManager saveManager;
		[SerializeField] private Toggle vsyncToggle;
		[SerializeField] private Toggle discordToggle;
		[SerializeField] private TMP_Dropdown grassRenderDistanceDropdown;
		[SerializeField] private TMP_Dropdown grassDensityDropdown;
		[SerializeField] private TMP_Dropdown qualityDropdown;
		[SerializeField] private TMP_Dropdown renderScalingDropdown;
		[SerializeField] private TMP_Dropdown shadowQualityDropdown;
		[SerializeField] private TMP_Dropdown antialiasingDropdown;
		[SerializeField] private TMP_Dropdown lodDropdown;
		[SerializeField] private TMP_Dropdown presetDropdown;
		private bool _isLoading;
		
		public void Start()
		{
			QualitySettings.vSyncCount = vsyncToggle.isOn ? 1 : 0;
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
					break;
				case 1:
					vsyncToggle.isOn = false;
					grassRenderDistanceDropdown.value = 1;
					grassDensityDropdown.value = 1;
					qualityDropdown.value = 1;
					renderScalingDropdown.value = 2;
					shadowQualityDropdown.value = 0;
					antialiasingDropdown.value = 0;
					lodDropdown.value = 0;
					break;
				case 2:
					vsyncToggle.isOn = true;
					grassRenderDistanceDropdown.value = 2;
					grassDensityDropdown.value = 2;
					qualityDropdown.value = 2;
					renderScalingDropdown.value = 2;
					shadowQualityDropdown.value = 1;
					antialiasingDropdown.value = 1;
					lodDropdown.value = 1;
					break;
				case 3:
					vsyncToggle.isOn = true;
					grassRenderDistanceDropdown.value = 2;
					grassDensityDropdown.value = 3;
					qualityDropdown.value = 3;
					renderScalingDropdown.value = 2;
					shadowQualityDropdown.value = 3;
					antialiasingDropdown.value = 2;
					lodDropdown.value = 2;
					break;
				case 4:
					vsyncToggle.isOn = true;
					grassRenderDistanceDropdown.value = 3;
					grassDensityDropdown.value = 3;
					qualityDropdown.value = 4;
					renderScalingDropdown.value = 2;
					shadowQualityDropdown.value = 4;
					antialiasingDropdown.value = 3;
					lodDropdown.value = 2;
					break;
				case 5:
					vsyncToggle.isOn = true;
					grassRenderDistanceDropdown.value = 4;
					grassDensityDropdown.value = 3;
					qualityDropdown.value = 4;
					renderScalingDropdown.value = 4;
					shadowQualityDropdown.value = 5;
					antialiasingDropdown.value = 3;
					lodDropdown.value = 3;
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
			qualityDropdown.value = configuration.Quality;
			renderScalingDropdown.value = configuration.RenderScaling;
			shadowQualityDropdown.value = configuration.ShadowQuality;
			antialiasingDropdown.value = configuration.AntiAliasing;
			lodDropdown.value = configuration.LodLevel;
			discordToggle.isOn = configuration.IsDiscordEnabled;
			
			gameObject.SetActive(true);
			_isLoading = false;
		}

		public void Close()
		{
			gameObject.SetActive(false);
		}
		
		public void SaveSettings()
		{
			var configuration = FindObjectOfType<SaveFile>().ConfigurationFile;
			
			configuration.PresetIndex = presetDropdown.value;
			configuration.Vsync = vsyncToggle.isOn;
			configuration.GrassRenderDistance = grassRenderDistanceDropdown.value;
			configuration.GrassDensity = grassDensityDropdown.value;
			configuration.Quality = qualityDropdown.value;
			configuration.RenderScaling = renderScalingDropdown.value;
			configuration.ShadowQuality = shadowQualityDropdown.value;
			configuration.AntiAliasing = antialiasingDropdown.value;
			configuration.LodLevel = lodDropdown.value;
			configuration.IsDiscordEnabled = discordToggle.isOn;
			
			saveManager.SaveGame();
			saveManager.ApplySettings();
			Close();
		}
	}
}