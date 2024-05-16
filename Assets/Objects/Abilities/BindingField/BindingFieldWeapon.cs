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

		public override void SetupProjectile(NetworkProjectile networkProjectile)
		{
			var randomEnemy = EnemyManager.instance.GetRandomEnemy();
			if (randomEnemy is null)
			{
				networkProjectile.Despawn(WeaponId);
				return;
			}

			var pointOnSurface = Utilities.GetPointOnColliderSurface(randomEnemy.transform.position, transform);
        
			networkProjectile.Initialize(this, pointOnSurface);
			networkProjectile.projectile.gameObject.SetActive(true);
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