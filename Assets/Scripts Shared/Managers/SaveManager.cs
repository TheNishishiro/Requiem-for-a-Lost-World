using System;
using System.Collections.Generic;
using System.IO;
using DefaultNamespace.Data;
using Objects.Characters;
using UI.Main_Menu.Character_List_Menu;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using ShadowQuality = UnityEngine.ShadowQuality;
using ShadowResolution = UnityEngine.ShadowResolution;

namespace Managers
{
	public class SaveManager : MonoBehaviour
	{
		private static bool _isFirstLoad = true;
		[SerializeField] private CharacterListMenu characterListMenu;
		private SaveFile _saveData;

		private void Awake()
		{
			_saveData = FindObjectOfType<SaveFile>();
			Time.timeScale = 1f;
			if (_isFirstLoad)
			{
				_isFirstLoad = false;
				LoadGame();
			}
		}

		public void SaveGame()
		{
			_saveData.UpdateMissingCharacterEntries(CharacterListManager.instance.GetCharacters());
			_saveData.UpdateMissingAchievementEntries();
			_saveData.Save();
		}

		public void LoadGame()
		{
			_saveData.Initialize();
			_saveData.Load();
			_saveData.UpdateMissingCharacterEntries(CharacterListManager.instance.GetCharacters());
			_saveData.UpdateMissingAchievementEntries();
			if (!_saveData.CharacterSaveData[CharactersEnum.Nishi].IsUnlocked)
				_saveData.CharacterSaveData[CharactersEnum.Nishi].Unlock();
			if (!_saveData.CharacterSaveData[CharactersEnum.Amelia].IsUnlocked)
				_saveData.CharacterSaveData[CharactersEnum.Amelia].Unlock();
			characterListMenu.UpdateCharacterPanels();
			ApplySettings();
		}

		public void ApplySettings()
		{
			var settings = _saveData.ConfigurationFile;
			
			var renderPipeline = (UniversalRenderPipelineAsset)QualitySettings.renderPipeline;
			
			QualitySettings.vSyncCount = settings.Vsync ? 1 : 0;

			renderPipeline.renderScale = settings.RenderScaling switch
			{
				0 => 0.25f,
				1 => 0.5f,
				2 => 0.75f,
				3 => 0.88f,
				4 => 1f,
				5 => 1.25f,
				6 => 1.5f,
				7 => 2f,
				_ => renderPipeline.renderScale
			};

			QualitySettings.lodBias = settings.LodLevel switch
			{
				0 => 0.4f,
				1 => 1f,
				2 => 2f,
				3 => 3f,
				4 => 4f,
				_ => QualitySettings.lodBias
			};

			switch (settings.Quality)
			{
				case 0:
					renderPipeline.supportsHDR = false;
					QualitySettings.realtimeReflectionProbes = false;
					QualitySettings.globalTextureMipmapLimit = 2;
					QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
					QualitySettings.particleRaycastBudget = 4;
					QualitySettings.billboardsFaceCameraPosition = false;
					QualitySettings.shadowmaskMode = ShadowmaskMode.Shadowmask;
					break;
				case 1:
					renderPipeline.supportsHDR = true;
					QualitySettings.realtimeReflectionProbes = false;
					QualitySettings.globalTextureMipmapLimit = 1;
					QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
					QualitySettings.particleRaycastBudget = 4;
					QualitySettings.billboardsFaceCameraPosition = false;
					QualitySettings.shadowmaskMode = ShadowmaskMode.Shadowmask;
					break;
				case 2:
					renderPipeline.supportsHDR = true;
					QualitySettings.realtimeReflectionProbes = false;
					QualitySettings.globalTextureMipmapLimit = 0;
					QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
					QualitySettings.particleRaycastBudget = 64;
					QualitySettings.billboardsFaceCameraPosition = false;
					QualitySettings.shadowmaskMode = ShadowmaskMode.Shadowmask;
					break;
				case 3:
					renderPipeline.supportsHDR = true;
					QualitySettings.realtimeReflectionProbes = false;
					QualitySettings.globalTextureMipmapLimit = 0;
					QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
					QualitySettings.particleRaycastBudget = 1024;
					QualitySettings.billboardsFaceCameraPosition = true;
					QualitySettings.shadowmaskMode = ShadowmaskMode.Shadowmask;
					break;
				case 4:
					renderPipeline.supportsHDR = true;
					QualitySettings.realtimeReflectionProbes = true;
					QualitySettings.globalTextureMipmapLimit = 0;
					QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
					QualitySettings.particleRaycastBudget = 2048;
					QualitySettings.billboardsFaceCameraPosition = true;
					QualitySettings.shadowmaskMode = ShadowmaskMode.DistanceShadowmask;
					break;
			}
			
			switch (settings.ShadowQuality)
			{
				case 0:
					QualitySettings.shadows = ShadowQuality.Disable;
					break;
				case 1:
					QualitySettings.shadowDistance = 50;
					QualitySettings.shadowCascades = 1;
					QualitySettings.shadows = ShadowQuality.HardOnly;
					QualitySettings.shadowResolution = ShadowResolution.Low;
					break;
				case 2:
					QualitySettings.shadowDistance = 100;
					QualitySettings.shadowCascades = 1;
					QualitySettings.shadows = ShadowQuality.HardOnly;
					QualitySettings.shadowResolution = ShadowResolution.Medium;
					break;
				case 3:
					QualitySettings.shadowDistance = 150;
					QualitySettings.shadowCascades = 1;
					QualitySettings.shadows = ShadowQuality.All;
					QualitySettings.shadowResolution = ShadowResolution.Medium;
					break;
				case 4:
					QualitySettings.shadowDistance = 150;
					QualitySettings.shadowCascades = 4;
					QualitySettings.shadows = ShadowQuality.All;
					QualitySettings.shadowResolution = ShadowResolution.High;
					break;
				case 5:
					QualitySettings.shadowDistance = 150;
					QualitySettings.shadowCascades = 4;
					QualitySettings.shadows = ShadowQuality.All;
					QualitySettings.shadowResolution = ShadowResolution.VeryHigh;
					break;
			}

			QualitySettings.antiAliasing = settings.AntiAliasing switch
			{
				0 => 0,
				1 => 2,
				2 => 4,
				3 => 8,
				_ => QualitySettings.antiAliasing
			};

			switch (settings.WindowMode)
			{
				case 0:
					Screen.fullScreen = true;
					Screen.SetResolution(settings.ResolutionWidth, settings.ResolutionHeight, true);
					break;
				case 1:
					Screen.fullScreen = false;
					Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
					Screen.SetResolution(settings.ResolutionWidth, settings.ResolutionHeight, FullScreenMode.FullScreenWindow);
					break;
				case 2:
					Screen.fullScreen = false;
					Screen.SetResolution(settings.ResolutionWidth, settings.ResolutionHeight, false);
					break;
			}
			
		}
	}
}