using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Objects.Environment;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
	public class DestructablesManager : MonoBehaviour
	{
		[SerializeField] private List<Destructable> destructables;
		[SerializeField] private Vector2 spawnArea;
		[SerializeField] private float spawnCooldown;
		[SerializeField] private int maxObjectsCount;
		private Player _player;
		private float _currentSpawnCooldown;

		public void Start()
		{
			_player = FindFirstObjectByType<Player>();
		}

		public void Update()
		{
			if (_currentSpawnCooldown > 0)
			{
				_currentSpawnCooldown -= Time.deltaTime;
				return;
			}

			if (FindObjectsOfType<Destructable>().Length >= maxObjectsCount)
			{
				_currentSpawnCooldown = spawnCooldown;
				return;
			}

			var position = _player.transform.position - Utilities.GenerateRandomPositionOnEdge(spawnArea);
			var pointFound = Utilities.GetPointOnColliderSurface(position, 100f, _player.transform, out var pointOnSurface);
			if (!pointFound)
			{
				_currentSpawnCooldown = spawnCooldown;
				return;
			}

			var destructable = SpawnManager.instance.SpawnObject(pointOnSurface, destructables.OrderBy(x => Random.value).First().gameObject);
			pointOnSurface.y += destructable.GetComponent<BoxCollider>().size.y/2;
			destructable.gameObject.transform.position = pointOnSurface;
			_currentSpawnCooldown = spawnCooldown;
		}
	}
}