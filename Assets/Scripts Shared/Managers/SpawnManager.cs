using UnityEngine;

namespace Managers
{
	public class SpawnManager : MonoBehaviour
	{
		public static SpawnManager instance;

		public void Awake()
		{
			if (instance == null)
			{
				instance = this;
			}
		}

		public GameObject SpawnObject(Vector3 worldPosition, GameObject gameObject)
		{
			var go = Instantiate(gameObject, transform);
			go.transform.position = worldPosition;

			return go;
		}

		public GameObject SpawnObject(Vector3 worldPosition, GameObject gameObject, Quaternion rotation)
		{
			var go = Instantiate(gameObject, worldPosition, rotation, transform);
			go.transform.position = worldPosition;

			return go;
		}
	}
}