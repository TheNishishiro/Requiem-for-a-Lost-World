using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using DefaultNamespace.BaseClasses;
using Managers;
using Objects.Enemies;
using Objects.Players.Scripts;
using Objects.Stage;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Pool;
using Weapons;

public class EnemyManager : NetworkBehaviour
{
	public static EnemyManager instance;
	[SerializeField] public GameObject enemyGameObject;
	[SerializeField] private List<EnemyData> defaultSpawns;
	[SerializeField] private List<EnemyData> possibleEnemies;
	[SerializeField] private Vector2 spawnArea;
	[SerializeField] private float spawnTimer;
	[SerializeField] private PlayerStatsComponent _playerStatsComponent;
	private static Transform PlayerTransform => GameManager.instance.PlayerTransform;
	private List<Enemy> _enemies = new ();
	public int currentEnemyCount => _enemies.Count;
	private int enemyMinCount;
	private int enemyMaxCount = 300;
	private float _timer;
	private bool _isTimeStop;
	private float _healthMultiplier = 1.0f;
	private EnemyData _currentEnemySpawning;
	private float _enemySpeedMultiplier = 1;
	public bool IsDisableEnemySpawn = true;

	public void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		
		defaultSpawns = new List<EnemyData>();
	}
	
	private void Update()
	{
		_timer -= Time.deltaTime;

		if (_timer <= 0 || currentEnemyCount < enemyMinCount)
			SpawnEnemy();
	}

	private void SpawnEnemy()
	{
		if (!defaultSpawns.Any())
			return;
		
		_timer = spawnTimer * PlayerStatsScaler.GetScaler().GetEnemySpawnRateIncrease() * GameData.GetCurrentDifficulty().EnemySpawnRateModifier;
		
		var randomSpawn = defaultSpawns[Random.Range(0, defaultSpawns.Count)];
		SpawnEnemy(randomSpawn);
	}

	public void SpawnEnemy(EnemyData enemyToSpawn)
	{
		if (!IsHost) return;
		if (PlayerTransform == null) return;

		var playerCount = NetworkManager.Singleton.ConnectedClients.Count;
		var increasePerClient = playerCount <= 1 ? 0 : playerCount * 0.5f;
		
		var maxEnemyCount = 
			enemyMaxCount * PlayerStatsScaler.GetScaler().GetEnemyCountIncrease() * GameData.GetCurrentDifficulty().EnemyCapacityModifier
			* (1 + increasePerClient);
		if (currentEnemyCount >= maxEnemyCount && !enemyToSpawn.isBossEnemy)
			return;
		if (IsDisableEnemySpawn)
			return;
		
		_currentEnemySpawning = enemyToSpawn;

		
		var targetClient = NetworkManager.Singleton.ConnectedClients.OrderBy(x => Random.value).FirstOrDefault().Value.PlayerObject.transform;
		var position = targetClient.position - Utilities.GenerateRandomPositionOnEdge(spawnArea);
		var pointFound = Utilities.GetPointOnColliderSurface(position, 100f, targetClient, out var pointOnSurface);
		if (!pointFound || Utilities.IsPositionOccupied(pointOnSurface, 0.3f))
			return;
		
		position = pointOnSurface;
		
		var enemy = NetworkObjectPool.Singleton.GetNetworkObject(enemyGameObject, position, Quaternion.identity);
		var enemyComponent = enemy.GetComponent<Enemy>();
		enemy.Spawn();

		
		var expIncrease = playerCount <= 1 ? 1 : Mathf.Pow(0.85f, playerCount);
		
		enemyComponent.SetPlayerTarget(targetClient);
		enemyComponent.Setup(new EnemyNetworkStats(_currentEnemySpawning), _healthMultiplier * (1 + increasePerClient*0.5f), _enemySpeedMultiplier, expIncrease);
		enemyComponent.gameObject.SetActive(true);
		if (_currentEnemySpawning.enemyName == "grand octi")
			enemyComponent.SetupBoss();
		
		RpcManager.instance.AddEnemyRpc(enemyComponent);
	}

	public void ChangeDefaultSpawn(List<EnemyData> enemyData)
	{
		defaultSpawns = enemyData;
	}

	public void ChangeSpawnRate(float timer)
	{
		spawnTimer = timer;
	}
	
	public void ChangeHealthMultiplier(float healthMultiplier)
	{
		_healthMultiplier = healthMultiplier;
	}

	public void BurstSpawn(List<EnemyData> stageEventEnemies, float stageEventCount)
	{
		StartCoroutine(BurtSpawnCoroutine(stageEventEnemies, stageEventCount));
	}
	
	private IEnumerator BurtSpawnCoroutine(List<EnemyData> stageEventEnemies, float stageEventCount)
	{
		for (var i = 0; i < stageEventCount; i++)
		{
			var randomEnemy = stageEventEnemies[Random.Range(0, stageEventEnemies.Count)];
			SpawnEnemy(randomEnemy);
		}

		yield break;
	}

	public void ChangeMinimumEnemyCount(float stageEventMinCount)
	{
		enemyMinCount = (int)stageEventMinCount;
	}

	public void EraseAllEnemies()
	{
		for (var i = 0; i < _enemies.Count; i++)
		{
			if (_enemies[i] != null)
			{
				Despawn(_enemies[i]);
				i--;
			}
		}
		_enemies.Clear();
	}

	public void Despawn(Enemy enemy)
	{
		if (!IsHost) return;
		
		RpcManager.instance.RemoveEnemyRpc(enemy);
	}

	public void SetTimeStop(bool isTimeStop)
	{
		_isTimeStop = isTimeStop;
	}

	public bool IsTimeStop()
	{
		return _isTimeStop;
	}

	public void GlobalDamage(float damage, WeaponBase weapon)
	{
		var enemies = FindObjectsByType<Damageable>(FindObjectsSortMode.None);
		foreach (var enemy in enemies)
		{
			enemy.TakeDamage(damage, weapon);
		}
	}

	public void DamageInView(float damage, WeaponBase weapon)
	{
		var enemiesToDamage = GetActiveEnemies().Where(x => Utilities.IsWithinCameraView(Camera.main, x.GetCollider().bounds,
			x.transform.position, PlayerTransform.position, 15f)).Select(x => x.GetDamagableComponent());
		
		foreach (var enemy in enemiesToDamage)
		{
			enemy.TakeDamage(damage, weapon);
		}
	}

	public IEnumerable<Enemy> GetActiveEnemies()
	{
		return _enemies;
	}

	public Enemy GetRandomEnemy()
	{
		var activeEnemies = GetActiveEnemies();
		return activeEnemies.OrderBy(_ => Random.value).FirstOrDefault();
	}

	public Enemy GetUncontrolledClosestEnemy(Vector3 position)
	{
		var activeEnemies = GetActiveEnemies();
		return activeEnemies.Where(x => !x.IsPlayerControlled()).OrderBy(enemy => Vector3.Distance(position, enemy.transform.position)).FirstOrDefault();
	}

	public void ChangeSpeedMultiplier(float speedMultiplier)
	{
		_enemySpeedMultiplier = speedMultiplier;
	}

	public Sprite GetSpriteByEnemy(EnemyTypeEnum enemyType)
	{
		return possibleEnemies.FirstOrDefault(x => x.enemyType == enemyType)?.sprite;
	}

	public void AddEnemy(Enemy networkBehaviour)
	{
		_enemies.Add(networkBehaviour);
	}

	public void RemoveEnemy(Enemy networkBehaviour)
	{
		_enemies.Remove(networkBehaviour);
	}
}