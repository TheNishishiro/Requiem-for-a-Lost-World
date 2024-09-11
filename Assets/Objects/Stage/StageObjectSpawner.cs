using System;
using System.Collections;
using DefaultNamespace;
using Unity.Netcode;
using UnityEngine;

public class StageObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject shrinePrefab;
    [SerializeField] private int shrineAmount = 35;
    [SerializeField] private float shrineSpawnRange = 250;
    [SerializeField] private float shrineMinDistance = 40;
    [SerializeField] private Vector3 mapCenter = new (0, 6, 0);

    private void Start()
    {
        StartCoroutine(WaitForNetworkPool());
    }

    private IEnumerator WaitForNetworkPool()
    {
        while (true)
        {
            if (NetworkObjectPool.Singleton != null && NetworkManager.Singleton != null && NetworkManager.Singleton.IsConnectedClient)
            {
                if (!NetworkManager.Singleton.IsHost)
                    yield break;
                
                var spawnedPositions = Utilities.GetPositionsOnSurfaceWithMinDistance(shrineAmount, mapCenter, shrineSpawnRange, shrineMinDistance, transform, 100);
                foreach (var position in spawnedPositions)
                {
                    NetworkObjectPool.Singleton.GetNetworkObject(shrinePrefab, position, Quaternion.identity).Spawn();
                }
                yield break;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}
