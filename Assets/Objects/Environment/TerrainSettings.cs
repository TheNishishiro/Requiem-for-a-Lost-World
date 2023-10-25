using System;
using DefaultNamespace.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Objects.Environment
{
	public class TerrainSettings : MonoBehaviour
	{
		[SerializeField] private Terrain terrain;
		private int detailIndex2D = 0;
		private int detailIndex3D = 1;
		
		private void Start()
		{
			var saveFile = FindObjectOfType<SaveFile>();
			if (saveFile == null)
			{
				Debug.Log("Save file not found");    
				return;
			}

			if (saveFile.ConfigurationFile.Use3dGrass)
				SwitchTo3DGrass();
			else
				SwitchTo2DGrass();
			
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
		
		public void SwitchTo2DGrass()
		{
			PopulateTerrainWithGrass(detailIndex2D, true);
		}

		public void SwitchTo3DGrass()
		{
			PopulateTerrainWithGrass(detailIndex3D, false);
		}

		private void PopulateTerrainWithGrass(int detailIndex, bool is2DGrass)
		{
			ClearTerrainGrass(detailIndex2D);
			ClearTerrainGrass(detailIndex3D);
			int[,] newDetailLayer = new int[terrain.terrainData.detailWidth, terrain.terrainData.detailHeight];

			// Randomly set grass details density.
			for (int y = 0; y < terrain.terrainData.detailHeight; y++)
			{
				for (int x = 0; x < terrain.terrainData.detailWidth; x++)
				{
					newDetailLayer[x, y] = Random.Range(300, 500);
				}
			}

			terrain.terrainData.SetDetailLayer(0, 0, detailIndex, newDetailLayer);
			terrain.Flush();
		}
		
		private void ClearTerrainGrass(int detailIndex)
		{
			int[,] emptyDetailLayer = new int[terrain.terrainData.detailWidth, terrain.terrainData.detailHeight];

			// Fill the array with zeroes.
			for (int y = 0; y < terrain.terrainData.detailHeight; y++)
			{
				for (int x = 0; x < terrain.terrainData.detailWidth; x++)
				{
					emptyDetailLayer[x, y] = 0;
				}
			}

			terrain.terrainData.SetDetailLayer(0, 0, detailIndex, emptyDetailLayer);
			terrain.Flush();
		}
	}
}