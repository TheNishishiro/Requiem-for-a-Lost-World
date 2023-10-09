using System;
using UnityEngine;

namespace DefaultNamespace.Data
{
	public class ConfigurationFile
	{
		public bool Vsync { get; set; }
		public int GrassRenderDistance { get; set; }
		public int GrassDensity { get; set; }
		public int ShadowQuality { get; set; }
		public int Quality { get; set; }
		public int LodLevel { get; set; }
		public int AntiAliasing { get; set; }
		public int RenderScaling { get; set; }
		public int PresetIndex { get; set; }
		public bool IsDiscordEnabled { get; set; }

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
			
			return this;
		}
	}
}