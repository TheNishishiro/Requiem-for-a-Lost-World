using System;
using UnityEngine;

namespace Objects.Enemies
{
	[CreateAssetMenu]
	public class EnemyData : ScriptableObject
	{
		public string enemyName;
		public GameObject animatedPrefab;
		public EnemyStats stats;
		public bool allowFlying;
		public bool isBossEnemy;
		public int ExpDrop;
	}
}