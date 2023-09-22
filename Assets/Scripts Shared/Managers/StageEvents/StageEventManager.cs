using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Managers.StageEvents
{
	public class StageEventManager : MonoBehaviour
	{
		[SerializeField] private StageData stageData;
		private EnemyManager _enemyManager;
		private StageTime _stageTime;
		private int _eventIndexer;

		private void Start()
		{
			_stageTime = GetComponent<StageTime>();
			_enemyManager = FindObjectOfType<EnemyManager>();
		}

		private void Update()
		{
			if (_eventIndexer >= stageData.stageEvents.Count) return;

			var stageEvent = stageData.stageEvents[_eventIndexer];
			if (_stageTime.time < stageEvent.triggerTime) return;

			if (stageEvent.EraseEnemies)
			{
				_enemyManager.EraseAllEnemies();
			}

			if (stageEvent.bossEnemy != null)
			{
				_enemyManager.SpawnEnemy(stageEvent.bossEnemy);
			}

			if (stageEvent.enemies?.Any() == true)
			{
				_enemyManager.ChangeDefaultSpawn(stageEvent.enemies);
			}
			
			if (stageEvent.spawnRate > 0)
			{
				_enemyManager.ChangeSpawnRate(stageEvent.spawnRate);
			}
			
			if (stageEvent.minCount > 0)
			{
				_enemyManager.ChangeMinimumEnemyCount(stageEvent.minCount);
			}

			if (stageEvent.healthMultiplier > 0)
			{
				_enemyManager.ChangeHealthMultiplier(stageEvent.healthMultiplier);
			}
			
			if (stageEvent.burstSpawnCount > 0)
			{
				_enemyManager.BurstSpawn(stageEvent.enemies, stageEvent.burstSpawnCount);
			}
			
			_eventIndexer++;
		}
	}
}