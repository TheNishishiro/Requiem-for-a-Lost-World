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

			switch (settings.RenderScaling)
			{
				case 0:
					renderPipeline.renderScale = 0.25f;
					break;
				case 1:
					renderPipeline.renderScale = 0.5f;
					break;
				case 2:
					renderPipeline.renderScale = 1f;
					break;
				case 3:
					renderPipeline.renderScale = 1.25f;
					break;
				case 4:
					renderPipeline.renderScale = 1.5f;
					break;
			}

			switch (settings.LodLevel)
			{
				case 0:
					QualitySettings.lodBias = 0.4f;
					break;
				case 1:
					QualitySettings.lodBias = 1f;
					break;
				case 2:
					QualitySettings.lodBias = 2f;
					break;
				case 3:
					QualitySettings.lodBias = 4f;
					break;
			}
			
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

			switch (settings.AntiAliasing)
			{
				case 0:
					QualitySettings.antiAliasing = 0;	
					break;
				case 1:
					QualitySettings.antiAliasing = 2;	
					break;
				case 2:
					QualitySettings.antiAliasing = 4;	
					break;
				case 3:
					QualitySettings.antiAliasing = 8;	
					break;
			}
		}
	}
}