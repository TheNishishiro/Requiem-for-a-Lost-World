using System;
using DefaultNamespace.Data;
using UnityEngine;

namespace Objects.Environment
{
	public class TerrainSettings : MonoBehaviour
	{
		[SerializeField] private Terrain terrain;
		
		private void Start()
		{
			var saveFile = FindObjectOfType<SaveFile>();
			if (saveFile == null)
				Debug.Log("Save file not found");

			switch (saveFile.ConfigurationFile.GrassRenderDistance)
			{
				case 0:
					terrain.detailObjectDistance = 10;
					break;
				case 1:
					terrain.detailObjectDistance = 20;
					break;
				case 2:
					terrain.detailObjectDistance = 30;
					break;
				case 3:
					terrain.detailObjectDistance = 60;
					break;
				case 4:
					terrain.detailObjectDistance = 180;
					break;
			}
			switch (saveFile.ConfigurationFile.GrassDensity)
			{
				case 0:
					terrain.detailObjectDensity = 0;
					break;
				case 1:
					terrain.detailObjectDensity = 0.2f;
					break;
				case 2:
					terrain.detailObjectDensity = 0.5f;
					break;
				case 3:
					terrain.detailObjectDensity = 1;
					break;
			}
		}
	}
}