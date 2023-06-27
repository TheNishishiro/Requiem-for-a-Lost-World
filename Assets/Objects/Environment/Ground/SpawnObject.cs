using Managers;
using UnityEngine;

namespace Objects.Environment.Ground
{
	public class SpawnObject : MonoBehaviour
	{
		[SerializeField] private GameObject objectToSpawn;
		[SerializeField] [Range(0f,1f)] private float probability;

		public void Spawn()
		{
			if (Random.value >= probability) return;
			
			var positionX = transform.position.x + (20f * Random.value);
			var positionZ = transform.position.z + (20f * Random.value);
			SpawnManager.instance.SpawnObject(new Vector3(positionX, 0, positionZ), objectToSpawn);
		}
	}
}