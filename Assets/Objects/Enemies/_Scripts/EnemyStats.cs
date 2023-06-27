using System;

namespace Objects.Enemies
{
	[Serializable]
	public class EnemyStats
	{
		public int hp;
		public int damage;
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