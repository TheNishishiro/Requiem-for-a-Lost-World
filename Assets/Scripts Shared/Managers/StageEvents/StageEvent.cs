using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Managers.StageEvents;
using Objects.Enemies;
using UnityEngine;

[Serializable]
public class StageEvent
{
	public List<EnemyData> enemies;
	public EnemyData bossEnemy;
	public float triggerTime;
	public float spawnRate;
	public float speedMultiplier;
	public float minCount;
	public float healthMultiplier;
	public int burstSpawnCount;
	public bool EraseEnemies;
}