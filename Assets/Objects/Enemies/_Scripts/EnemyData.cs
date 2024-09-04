using System;
using Objects.Enemies.EnemyWeapons;
using UnityEngine;

namespace Objects.Enemies
{
	[CreateAssetMenu]
	public class EnemyData : ScriptableObject
	{
		public EnemyTypeEnum enemyType;
		public EnemyWeaponId[] enemyWeapons;
		public string enemyName;
		public Sprite spriteSheet;
		public EnemyStats stats;
		public bool allowFlying;
		public float groundOffset;
		public bool isBossEnemy;
		public int ExpDrop;
	}
}