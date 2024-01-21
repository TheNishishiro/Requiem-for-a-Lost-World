using System.Collections;
using System.Linq;
using DefaultNamespace;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Managers;
using Objects.Enemies;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.BindingField
{
	public class BindingFieldWeapon : PoolableWeapon<BindingFieldProjectile>
	{
		public double ChanceToBind { get; private set; } = 0.5f;
		public bool IsBurstDamage { get; private set; }

		protected override bool ProjectileSpawn(BindingFieldProjectile projectile)
		{
			var randomEnemy = EnemyManager.instance.GetRandomEnemy();
			if (randomEnemy == null)
				return false;

			var pointOnSurface = Utilities.GetPointOnColliderSurface(new Vector3(randomEnemy.transform.position.x, 0, randomEnemy.transform.position.z), transform);
			projectile.transform.position = pointOnSurface;
			projectile.SetParentWeapon(this);
			return true;
		}

		protected override IEnumerator AttackProcess()
		{
			Attack();
			yield break;
		}

		protected override void OnLevelUp()
		{
			switch (LevelField)
			{
				case 6:
					IsBurstDamage = true;
					break;
				case 7:
					ChanceToBind = 0.8f;
					break;
			}
		}
	}
}