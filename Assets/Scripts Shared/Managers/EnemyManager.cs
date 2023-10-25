using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using DefaultNamespace.BaseClasses;
using Managers;
using Objects.Enemies;
using Objects.Players.Scripts;
using UnityEngine;
using Weapons;

public class EnemyManager : Singleton<EnemyManager>
{
	[SerializeField] private GameObject enemyGameObject;
	[SerializeField] private List<EnemyData> defaultSpawns;
	[SerializeField] private Vector2 spawnArea;
	[SerializeField] private float spawnTimer;
	[SerializeField] private Player player;
	[SerializeField] private PlayerStatsComponent _playerStatsComponent;
	private List<Enemy> _enemies;
	public int currentEnemyCount;
	private int enemyMinCount;
	private int enemyMaxCount = 300;
	private float _timer;
	private bool _isTimeStop;
	private float _healthMultiplier = 1.0f;

	protected override void Awake()
	{
		base.Awake();
		currentEnemyCount = 0;
		defaultSpawns = new List<EnemyData>();
		_enemies = new List<Enemy>();
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
		
		_timer = spawnTimer * _playerStatsComponent.GetEnemySpawnRateIncrease();
		
		var randomSpawn = defaultSpawns[Random.Range(0, defaultSpawns.Count)];
		SpawnEnemy(randomSpawn);
	}

	public void SpawnEnemy(EnemyData enemyToSpawn)
	{
		var maxEnemyCount = enemyMaxCount * _playerStatsComponent.GetEnemyCountIncrease();
		if (currentEnemyCount >= maxEnemyCount && !enemyToSpawn.isBossEnemy)
			return;
		
		var position = player.transform.position - Utilities.GenerateRandomPositionOnEdge(spawnArea);
		var pointFound = Utilities.GetPointOnColliderSurface(position, 100f, player.transform, out var pointOnSurface);
		if (!pointFound)
			return;
		
		position = pointOnSurface;
		position.y += enemyToSpawn.animatedPrefab.GetComponent<BoxCollider>().size.y/2;
		var newEnemy = Instantiate(enemyGameObject);
		newEnemy.transform.position = position;
		var enemy = newEnemy.GetComponent<Enemy>();
		newEnemy.transform.parent = transform;

		var enemySprite = Instantiate(enemyToSpawn.animatedPrefab);
		enemySprite.transform.parent = newEnemy.transform;
		enemySprite.transform.localPosition = Vector3.zero;
		
		enemy.Setup(enemyToSpawn, player, this, _playerStatsComponent, _healthMultiplier);
		_enemies.Add(enemy);
		currentEnemyCount++;
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

	public void EnemyDespawn(Enemy enemy)
	{
		currentEnemyCount--;
		_enemies.Remove(enemy);
	}

	public void EraseAllEnemies()
	{
		var enemies = GetComponentsInChildren<Enemy>();
		foreach (var enemy in enemies)
		{
			Destroy(enemy.gameObject);
		}
		currentEnemyCount = 0;
		_enemies.Clear();
	}

	public void SetTimeStop(bool isTimeStop)
	{
		_isTimeStop = isTimeStop;
	}

	public bool IsTimeStop()
	{
		return _isTimeStop;
	}

	public List<Enemy> GetActiveEnemies()
	{
		return _enemies;
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