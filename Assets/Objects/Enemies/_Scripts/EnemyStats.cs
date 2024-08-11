using System;
using System.Collections.Generic;
using System.Linq;
using Data.Elements;

namespace Objects.Enemies
{
	[Serializable]
	public class EnemyStats
	{
		public float hp;
		public float damage;
		public float speed;
		public List<ElementStats> elementStats = new();

		public void Copy(EnemyStats enemyStats)
		{
			hp = enemyStats.hp;
			damage = enemyStats.damage;
			speed = enemyStats.speed;
			elementStats = enemyStats.elementStats?.ToList() ?? new List<ElementStats>();
		}

		public void Copy(EnemyNetworkStats enemyStats)
		{
			hp = enemyStats.hp;
			damage = enemyStats.damage;
			speed = enemyStats.speed;
			elementStats = enemyStats.elementStats.ToList();
		}
	}
}