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
		private Vector3 weaponCenter;
		private RealityShatterWeapon RealityShatterWeapon => (RealityShatterWeapon) ParentWeapon;
		[SerializeField] private GameObject slashPrefab;

		private void OnEnable()
		{
			StartCoroutine(AnimateAttack());
			var position = ParentWeapon.transform.position;
			weaponCenter = new Vector3(position.x, position.y, position.z);
		}

		private IEnumerator AnimateAttack()
		{
			EnemyManager.instance.SetTimeStop(true);
			yield return new WaitForSeconds(0.5f);
			SpawnSlash();
			yield return new WaitForSeconds(0.5f);
			EnemyManager.instance.SetTimeStop(false);
			Explode();
			yield return new WaitForSeconds(1f);
			Destroy();
		}

		private void SpawnSlash()
		{
			SpawnManager.instance.SpawnObject(weaponCenter, slashPrefab);
		}
		
		private void Explode()
		{
			foreach (var childRigidBody in GetComponentsInChildren<Rigidbody>())
			{
				childRigidBody.useGravity = true;
				childRigidBody.AddExplosionForce(5f, weaponCenter, 5f, 2f, ForceMode.Impulse);
			}
			
			if (RealityShatterWeapon.IsGlobalDamage)
				EnemyManager.instance.GlobalDamage(WeaponStatsStrategy.GetDamage() / 2, ParentWeapon);
			if (RealityShatterWeapon.IsSelfBuff)
				RealityShatterWeapon.IncreaseDamage();
		}

		public void OnEnemyHit(Damageable damageable)
		{
			if (WeaponStatsStrategy.GetWeakness() > 0)
				damageable.SetVulnerable(WeaponStatsStrategy.GetWeakness(), 5);
			SimpleDamage(damageable, false, false);
		}
	}
}