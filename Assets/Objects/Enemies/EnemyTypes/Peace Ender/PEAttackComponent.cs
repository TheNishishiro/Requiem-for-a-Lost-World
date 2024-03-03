using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Managers;
using Objects.Stage;
using Unity.Netcode;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Objects.Enemies.Peace_Ender
{
	public class PEAttackComponent : MonoBehaviour
	{
		[SerializeField] private GameObject lightPillarPrefab;
		[SerializeField] private GameObject indicatorPrefab;
		[SerializeField] private int lightPillarCount;
		[SerializeField] private float lightPillarAttackDelay;
		[SerializeField] private float lightPillarAttackArea;
		[SerializeField] private float attackCooldown;
		[SerializeField] public List<EnemyData> spawnableEnemies;
		[SerializeField] private int enemySpawnCountMin;
		[SerializeField] private int enemySpawnCountMax;
		private float _currentCooldown;
		
		public void Update()
		{
			_currentCooldown -= Time.deltaTime;
			if (_currentCooldown > 0) return;
			
			_currentCooldown = attackCooldown;
			var randomValue = Random.Range(0, 100);

			if (randomValue < 80)
			{
				LightPillarAttack();
			}
			else
			{
				SpawnEnemies();
			}
		}

		private void LightPillarAttack()
		{
			for (var i = 0; i < lightPillarCount; i++)
			{
				var point = Utilities.GetRandomInAreaFreezeParameter(transform.position, lightPillarAttackArea, isFreezeY: true);
				var attackPosition = Utilities.GetPointOnColliderSurface(point, transform, 0.2f); 
				
				var indicator = SpawnManager.instance.SpawnObject(attackPosition, indicatorPrefab);
                StartCoroutine(SpawnPillar(indicator, attackPosition));
			}
		}
		
		private IEnumerator SpawnPillar(Object indicator, Vector3 position)
		{
			yield return new WaitForSeconds(lightPillarAttackDelay);
			Destroy(indicator);
			SpawnManager.instance.SpawnObject(position, lightPillarPrefab);
		}

		private void SpawnEnemies()
		{
			EnemyManager.instance.BurstSpawn(spawnableEnemies, Random.Range(enemySpawnCountMin, enemySpawnCountMax));
		}
	}
}