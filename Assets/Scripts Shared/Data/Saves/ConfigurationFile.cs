using System;
using System.Diagnostics;
using UnityEngine;

namespace DefaultNamespace.Data
{
	public class ConfigurationFile
	{
		public int ConfigurationVersion { get; set; }
		public bool Vsync { get; set; }
		public int WindowMode { get; set; }
		public int GrassRenderDistance { get; set; }
		public int GrassDensity { get; set; }
		public int ShadowQuality { get; set; }
		public int Quality { get; set; }
		public int LodLevel { get; set; }
		public int AntiAliasing { get; set; }
		public int RenderScaling { get; set; }
		public int PresetIndex { get; set; }
		public bool IsDiscordEnabled { get; set; }
		public int ResolutionWidth { get; set; }
		public int ResolutionHeight { get; set; }
		public uint RefreshRate { get; set; }
		public int RenderDistance { get; set; }
		public bool Use3dGrass { get; set; }
		public int ObjectDensity { get; set; }

		public ConfigurationFile Default()
		{
			Vsync = true;
			Quality = 4;
			GrassDensity = 3;
			GrassRenderDistance = 3;
			RenderScaling = 2;
			ShadowQuality = 5;
			LodLevel = 2;
			AntiAliasing = 4;
			IsDiscordEnabled = true;
			PresetIndex = 4;
			ConfigurationVersion = 0;
			RenderDistance = 1;
			Use3dGrass = false;
			ObjectDensity = 1;
			return Update();
		}

		public ConfigurationFile Update()
		{
			if (ConfigurationVersion == 0)
			{
				WindowMode = 0;
				ResolutionWidth = Screen.currentResolution.width;
				ResolutionHeight = Screen.currentResolution.height;
				RefreshRate = Screen.currentResolution.refreshRateRatio.numerator;
				ConfigurationVersion = 1;
			}
			if (ConfigurationVersion == 1)
			{
				switch(RenderScaling)
				{
					case >= 2 and <= 4:
						RenderScaling += 2;
						break; 
				}
				ConfigurationVersion = 2;
			}
			if (ConfigurationVersion == 2)
			{
				if (LodLevel == 3)
					LodLevel = 4;
				ConfigurationVersion = 3;
			}
			if (ConfigurationVersion == 3)
			{
				RenderDistance = 1;
				ConfigurationVersion = 4;
			}
			if (ConfigurationVersion == 4)
			{
				ObjectDensity = 1;
				ConfigurationVersion = 5;
			}

			return this;
		}
	}
}