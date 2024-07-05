using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnOverTime : MonoBehaviour
{
    [SerializeField] private GameObject[] gameObjects;
    [SerializeField] private float timeBetweenSpawns;
    [SerializeField] private Transform spawnTransform;
    private Transform _transformCache;
    private float _currentTime;

    private void Start()
    {
        _transformCache = transform;
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime >= timeBetweenSpawns)
        {
            _currentTime = 0f;
            Instantiate(gameObjects.OrderBy(_ => Random.value).FirstOrDefault(), _transformCache.position, Quaternion.identity, spawnTransform);
        }
    }
}
