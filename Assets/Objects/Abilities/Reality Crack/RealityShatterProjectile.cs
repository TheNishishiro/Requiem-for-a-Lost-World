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
	public class RealityShatterProjectile : ProjectileBase
	{
		private Vector3 weaponCenter;
		private EnemyManager enemyManager;
		private RealityShatterWeapon RealityShatterWeapon => (RealityShatterWeapon) ParentWeapon;
		[SerializeField] private GameObject slashPrefab;
		[SerializeField] private Collider collider;
		
		private void Start()
		{
			enemyManager = FindObjectOfType<EnemyManager>();
			var position = ParentWeapon.transform.position;
			weaponCenter = new Vector3(position.x, position.y, position.z);
			StartCoroutine(AnimateAttack());
		}

		private IEnumerator AnimateAttack()
		{
			collider.enabled = true;
			enemyManager.SetTimeStop(true);
			yield return new WaitForSeconds(0.5f);
			SpawnSlash();
			yield return new WaitForSeconds(0.5f);
			enemyManager.SetTimeStop(false);
			Explode();
			collider.enabled = false;
			yield return new WaitForSeconds(1f);
			Destroy(gameObject);
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
				enemyManager.GlobalDamage(WeaponStatsStrategy.GetDamage() / 2, ParentWeapon);
			if (RealityShatterWeapon.IsSelfBuff)
				RealityShatterWeapon.IncreaseDamage();
		}
		
		public void OnTriggerStay(Collider other)
		{
			var damageable = other.GetComponent<IDamageable>();
			if (damageable != null)
			{
				if (WeaponStatsStrategy.GetWeakness() > 0)
					damageable.SetVulnerable(WeaponStatsStrategy.GetWeakness(), 5);
				damageable.TakeDamageWithCooldown(WeaponStatsStrategy.GetDamage(), gameObject, 0.2f, ParentWeapon);
			}
		}
	}
}