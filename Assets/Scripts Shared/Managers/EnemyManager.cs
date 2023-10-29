using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using DefaultNamespace.BaseClasses;
using Managers;
using Objects.Enemies;
using Objects.Players.Scripts;
using UnityEngine;
using UnityEngine.Pool;
using Weapons;

public class EnemyManager : Singleton<EnemyManager>
{
	[SerializeField] private GameObject enemyGameObject;
	[SerializeField] private List<EnemyData> defaultSpawns;
	[SerializeField] private Vector2 spawnArea;
	[SerializeField] private float spawnTimer;
	[SerializeField] private Player player;
	[SerializeField] private PlayerStatsComponent _playerStatsComponent;
	private ObjectPool<Enemy> enemyPool;
	private List<Enemy> _enemies;
	public int currentEnemyCount;
	private int enemyMinCount;
	private int enemyMaxCount = 300;
	private float _timer;
	private bool _isTimeStop;
	private float _healthMultiplier = 1.0f;
	private EnemyData _currentEnemySpawning;

	protected override void Awake()
	{
		base.Awake();
		currentEnemyCount = 0;
		defaultSpawns = new List<EnemyData>();
		_enemies = new List<Enemy>();
		enemyPool = new ObjectPool<Enemy>(OnCreateEnemy, OnRequestEnemy, OnEnemyRelease, Destroy, true, 600, 1000);
	}

	#region Pooling methods

	private Enemy OnCreateEnemy()
	{
		return Instantiate(enemyGameObject, transform).GetComponent<Enemy>();
	}

	private void OnRequestEnemy(Enemy enemy)
	{
		var position = player.transform.position - Utilities.GenerateRandomPositionOnEdge(spawnArea);
		var pointFound = Utilities.GetPointOnColliderSurface(position, 100f, player.transform, out var pointOnSurface);
		if (!pointFound)
			return;
		
		position = pointOnSurface;

		
		enemy.transform.position = position;
		enemy.Setup(_currentEnemySpawning, player, this, _playerStatsComponent, _healthMultiplier, _currentEnemySpawning.sprite);
		if (_currentEnemySpawning.enemyName == "grand octi")
			enemy.SetupBoss();
		
		currentEnemyCount++;
		enemy.gameObject.SetActive(true);
	}

	private void OnEnemyRelease(Enemy enemy)
	{
		enemy.gameObject.SetActive(false);
		currentEnemyCount--;
		_enemies.Remove(enemy);
	}

	#endregion
	
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
		
		_timer = spawnTimer * _playerStatsComponent.GetEnemySpawnRateIncrease();
		
		var randomSpawn = defaultSpawns[Random.Range(0, defaultSpawns.Count)];
		SpawnEnemy(randomSpawn);
	}

	public void SpawnEnemy(EnemyData enemyToSpawn)
	{
		var maxEnemyCount = enemyMaxCount * _playerStatsComponent.GetEnemyCountIncrease();
		if (currentEnemyCount >= maxEnemyCount && !enemyToSpawn.isBossEnemy)
			return;

		_currentEnemySpawning = enemyToSpawn;
		enemyPool.Get();
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
		enemyPool.Clear();
		currentEnemyCount = 0;
		_enemies.Clear();
	}

	public void Despawn(Enemy enemy)
	{
		enemyPool.Release(enemy);
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
}