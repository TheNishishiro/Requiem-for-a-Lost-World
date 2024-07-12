using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Interfaces;
using Managers;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Reality_Crack
{
	public class RealityShatterProjectile : PoolableProjectile<RealityShatterProjectile>
	{
		private RealityShatterWeapon RealityShatterWeapon => (RealityShatterWeapon) ParentWeapon;
		private bool _isExploded;

		public override void SetStats(IWeaponStatsStrategy weaponStatsStrategy)
		{
			base.SetStats(weaponStatsStrategy);
			_isExploded = false;
		}
		
		protected override void CustomUpdate()
		{
			if (TimeAlive > 1f && !_isExploded)
				Explode();
		}

		private void Explode()
		{
			_isExploded = true;
			foreach (var childRigidBody in GetComponentsInChildren<Rigidbody>())
			{
				childRigidBody.useGravity = true;
				childRigidBody.AddExplosionForce(5f, transform.position, 5f, 2f, ForceMode.Impulse);
			}
			
			if (RealityShatterWeapon.IsGlobalDamage)
				EnemyManager.instance.GlobalDamage(WeaponStatsStrategy.GetDamage() / 2, ParentWeapon);
			if (RealityShatterWeapon.IsSelfBuff)
				RealityShatterWeapon.IncreaseDamage();
		}

		public void OnEnemyHit(Damageable damageable)
		{
			if (WeaponStatsStrategy.GetWeakness() > 0)
				damageable.SetVulnerable(5, WeaponStatsStrategy.GetWeakness());
			SimpleDamage(damageable, false, false);
		}
	}
}