using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Objects.Environment
{
	public class ObjectSpawner : MonoBehaviour
	{
		[SerializeField] private List<SpawnGroup> spawnGroups;
		[SerializeField] private float spawnRadiusX;
		[SerializeField] private float spawnRadiusY;
		
		private void Start()
		{
			foreach (var spawnGroup in spawnGroups)
			{
				for (var i = 0; i < spawnGroup.spawnCount; i++)
				{
					var randomObject = spawnGroup.spawnableObjects[Random.Range(0, spawnGroup.spawnableObjects.Count)];
					var randomPosition = new Vector3(Random.Range(-spawnRadiusX, spawnRadiusX), 0, Random.Range(-spawnRadiusY, spawnRadiusY));
					Instantiate(randomObject, randomPosition, Quaternion.identity);
				}
			}
			
			
		}
	}
	
	[Serializable]
	public class SpawnGroup
	{
		public List<GameObject> spawnableObjects;
		public float spawnCount;
	}
}