using System;
using System.Collections.Generic;
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
			elementStats ??= new List<ElementStats>();
			elementStats.Clear();
			enemyStats.elementStats.ForEach(x =>
			{
				elementStats?.Add(new ElementStats(x));
			});
		}
	}
}