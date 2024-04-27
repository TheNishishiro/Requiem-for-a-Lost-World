using System;
using System.Collections.Generic;
using System.IO;
using DefaultNamespace.Data;
using Objects.Characters;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using ShadowQuality = UnityEngine.ShadowQuality;
using ShadowResolution = UnityEngine.ShadowResolution;

namespace Managers
{
	public class SaveManager : MonoBehaviour
	{
		private static bool _isFirstLoad = true;
		public static SaveManager instance;

		private void Awake()
		{
			if (instance == null)
			{
				instance = this;
			}
			
			Time.timeScale = 1f;
			if (_isFirstLoad)
			{
				_isFirstLoad = false;
				LoadGame();
			}
		}

		public SaveFile GetSaveFile()
		{
			return SaveFile.Instance;
		}

		public void SaveGame()
		{
			SaveFile.Instance.UpdateMissingCharacterEntries(CharacterListManager.instance.GetCharacters());
			SaveFile.Instance.UpdateMissingAchievementEntries();
			SaveFile.Instance.Save();
		}

		public void LoadGame()
		{
			SaveFile.Instance.Initialize();
			SaveFile.Instance.Load();
			SaveFile.Instance.UpdateMissingCharacterEntries(CharacterListManager.instance.GetCharacters());
			SaveFile.Instance.UpdateMissingAchievementEntries();
			if (!SaveFile.Instance.CharacterSaveData[CharactersEnum.Nishi].IsUnlocked)
				SaveFile.Instance.CharacterSaveData[CharactersEnum.Nishi].Unlock();
			if (!SaveFile.Instance.CharacterSaveData[CharactersEnum.Amelia].IsUnlocked)
				SaveFile.Instance.CharacterSaveData[CharactersEnum.Amelia].Unlock();
			ApplySettings();
		}

		public void ApplySettings()
		{
			var settings = SaveFile.Instance.ConfigurationFile;

			AudioListener.volume = settings.Volume;
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
				1 => 0.8f,
				2 => 1f,
				3 => 2f,
				4 => 3f,
				5 => 4f,
				6 => 5f,
				_ => QualitySettings.lodBias
			};

			QualitySettings.globalTextureMipmapLimit = settings.TextureQuality switch
			{
				0 => 4,
				1 => 3,
				2 => 2,
				3 => 1,
				4 => 0,
				_ => QualitySettings.globalTextureMipmapLimit
			};
			
			switch (settings.Quality)
			{
				case 0:
					renderPipeline.supportsHDR = false;
					QualitySettings.realtimeReflectionProbes = false;
					QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
					QualitySettings.particleRaycastBudget = 4;
					QualitySettings.billboardsFaceCameraPosition = false;
					QualitySettings.shadowmaskMode = ShadowmaskMode.Shadowmask;
					break;
				case 1:
					renderPipeline.supportsHDR = true;
					QualitySettings.realtimeReflectionProbes = false;
					QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
					QualitySettings.particleRaycastBudget = 4;
					QualitySettings.billboardsFaceCameraPosition = false;
					QualitySettings.shadowmaskMode = ShadowmaskMode.Shadowmask;
					break;
				case 2:
					renderPipeline.supportsHDR = true;
					QualitySettings.realtimeReflectionProbes = false;
					QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
					QualitySettings.particleRaycastBudget = 64;
					QualitySettings.billboardsFaceCameraPosition = false;
					QualitySettings.shadowmaskMode = ShadowmaskMode.Shadowmask;
					break;
				case 3:
					renderPipeline.supportsHDR = true;
					QualitySettings.realtimeReflectionProbes = false;
					QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
					QualitySettings.particleRaycastBudget = 1024;
					QualitySettings.billboardsFaceCameraPosition = true;
					QualitySettings.shadowmaskMode = ShadowmaskMode.Shadowmask;
					break;
				case 4:
					renderPipeline.supportsHDR = true;
					QualitySettings.realtimeReflectionProbes = true;
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
					renderPipeline.shadowDistance = 5f;
					renderPipeline.shadowCascadeCount = 1;
					QualitySettings.shadows = ShadowQuality.HardOnly;
					QualitySettings.shadowResolution = ShadowResolution.Low;
					break;
				case 2:
					renderPipeline.shadowDistance = 20;
					renderPipeline.shadowCascadeCount = 1;
					QualitySettings.shadows = ShadowQuality.HardOnly;
					QualitySettings.shadowResolution = ShadowResolution.Medium;
					break;
				case 3:
					renderPipeline.shadowDistance = 50;
					renderPipeline.shadowCascadeCount = 1;
					QualitySettings.shadows = ShadowQuality.All;
					QualitySettings.shadowResolution = ShadowResolution.Medium;
					break;
				case 4:
					renderPipeline.shadowDistance = 100;
					renderPipeline.shadowCascadeCount = 2;
					QualitySettings.shadows = ShadowQuality.All;
					QualitySettings.shadowResolution = ShadowResolution.High;
					break;
				case 5:
					renderPipeline.shadowDistance = 200;
					renderPipeline.shadowCascadeCount = 4;
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
			
			if (Application.platform == RuntimePlatform.Android) return;
			
			switch (settings.WindowMode)
			{
				case 0:
					Screen.fullScreen = true;
					Screen.SetResolution(settings.ResolutionWidth, settings.ResolutionHeight, true);
					break;
				case 1:
					Screen.fullScreen = false;
					Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
					Screen.SetResolution(settings.ResolutionWidth, settings.ResolutionHeight,
						FullScreenMode.FullScreenWindow);
					break;
				case 2:
					Screen.fullScreen = false;
					Screen.SetResolution(settings.ResolutionWidth, settings.ResolutionHeight, false);
					break;
			}

		}
	}
}