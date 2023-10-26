using System;
using System.Collections.Generic;
using DefaultNamespace.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Objects.Environment
{
	public class TerrainSettings : MonoBehaviour
	{
		[SerializeField] private Terrain terrain;
		private int treePrototypeIndex = 0;

		private int detailIndex2D = 0;
		private int detailIndex3D = 1;
		private int detailIndex3D_Flowers = 2;

		private float _treeDensity;

		private void Start()
		{
			var saveFile = FindObjectOfType<SaveFile>();
			if (saveFile == null)
			{
				Debug.Log("Save file not found");
				return;
			}

			_treeDensity = saveFile.ConfigurationFile.ObjectDensity switch
			{
				0 => 0.5f,
				1 => 1,
				2 => 1.3f,
				3 => 1.7f,
				_ => 1
			};

			terrain.detailObjectDistance = saveFile.ConfigurationFile.GrassRenderDistance switch
			{
				0 => 10,
				1 => 20,
				2 => 30,
				3 => 60,
				4 => 180,
				_ => terrain.detailObjectDistance
			};

			terrain.detailObjectDensity = saveFile.ConfigurationFile.GrassDensity switch
			{
				0 => 0,
				1 => 0.2f,
				2 => 0.5f,
				3 => saveFile.ConfigurationFile.Use3dGrass ? 0.65f : 0.8f,
				4 => 1,
				_ => terrain.detailObjectDensity
			};

			RemoveTrees(treePrototypeIndex);
			PlaceTrees(treePrototypeIndex);

			if (saveFile.ConfigurationFile.Use3dGrass)
				SwitchTo3DGrass();
			else
				SwitchTo2DGrass();
		}

		private void SwitchTo2DGrass()
		{
			Clear();
			PopulateTerrainWithGrass(detailIndex2D, 300, 500);
		}

		private void SwitchTo3DGrass()
		{
			Clear();
			PopulateTerrainWithGrass(detailIndex3D, 300, 500);
			PopulateTerrainWithGrass(detailIndex3D_Flowers, 10, 20);
		}

		private void PopulateTerrainWithGrass(int detailIndex, int minAmount, int maxAmount)
		{

			var newDetailLayer = new int[terrain.terrainData.detailWidth, terrain.terrainData.detailHeight];
			for (var y = 0; y < terrain.terrainData.detailHeight; y++)
			{
				for (var x = 0; x < terrain.terrainData.detailWidth; x++)
				{
					newDetailLayer[x, y] = Random.Range(minAmount, maxAmount);
				}
			}

			terrain.terrainData.SetDetailLayer(0, 0, detailIndex, newDetailLayer);
			terrain.Flush();
		}

		private void Clear()
		{
			ClearTerrainGrass(detailIndex2D);
			ClearTerrainGrass(detailIndex3D);
			ClearTerrainGrass(detailIndex3D_Flowers);
		}

		private void ClearTerrainGrass(int detailIndex)
		{
			var emptyDetailLayer = new int[terrain.terrainData.detailWidth, terrain.terrainData.detailHeight];
			for (var y = 0; y < terrain.terrainData.detailHeight; y++)
			{
				for (var x = 0; x < terrain.terrainData.detailWidth; x++)
				{
					emptyDetailLayer[x, y] = 0;
				}
			}

			terrain.terrainData.SetDetailLayer(0, 0, detailIndex, emptyDetailLayer);
			terrain.Flush();
		}

		private void RemoveTrees(int prototypeIndex)
		{
			var trees = new List<TreeInstance>(terrain.terrainData.treeInstances);
			trees.RemoveAll(tree => tree.prototypeIndex == prototypeIndex);
			terrain.terrainData.treeInstances = trees.ToArray();
			terrain.terrainData.RefreshPrototypes();
		}

		private void PlaceTrees(int prototypeIndex)
		{
			for (var i = 0; i < 6500 * _treeDensity; i++)
			{
				var tree = new TreeInstance
				{
					prototypeIndex = prototypeIndex,
					widthScale = Random.Range(0.8f, 1.2f),
					heightScale = Random.Range(0.6f, 1.4f),
					color = Color.white,
					lightmapColor = Color.white,
					position = new Vector3(Random.Range(0f, 1f), 0, Random.Range(0f, 1f))
				};
				terrain.AddTreeInstance(tree);
			}
		}
	}
}