using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Managers.StageEvents;
using Objects.Enemies;
using UnityEngine;

[Serializable]
public class StageEvent
{
	public List<EnemySpawnData> spawnableEnemies;
	public EnemyData bossEnemy;
	public float triggerTime;
	public float spawnRate;
	public float speedMultiplier;
	public float minCount;
	public float healthMultiplier;
	public float spawnRange;
	public int burstSpawnCount;
	public bool EraseEnemies;
	public float damageMultiplier;
}