using System;

namespace Objects.Enemies
{
	[Serializable]
	public class EnemyStats
	{
		public float hp;
		public float damage;
		public float speed;

		public EnemyStats()
		{
		}

		public EnemyStats(EnemyStats enemyStats)
		{
			hp = enemyStats.hp;
			damage = enemyStats.damage;
			speed = enemyStats.speed;
		}
	}
}