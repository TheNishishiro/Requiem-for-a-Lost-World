using System;
using Interfaces;
using Managers;
using Objects.Abilities.SpaceExpansionBall;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Book
{
	public class BookProjectile : PoolableProjectile<BookProjectile>
	{
		private GameObject rotateTarget;
		private BookWeapon BookWeapon => ParentWeapon as BookWeapon;

		public void SetTarget(GameObject rotateTarget)
		{
			this.rotateTarget = rotateTarget;
		}
		
		private void Update()
		{
			if (rotateTarget != null)
			{
				transform.RotateAround(rotateTarget.transform.position, Vector3.up, Time.deltaTime * WeaponStats.GetSpeed());
			}
			
			TickLifeTime();
		}

		private void SpawnExplosion()
		{
			var explosion = SpawnManager.instance.SpawnObject(transform.position, BookWeapon.ExplosionPrefab);
			var simpleDamageProjectile = explosion.GetComponent<SimpleDamageProjectile>();
			simpleDamageProjectile.SetParentWeapon(ParentWeapon);
			simpleDamageProjectile.SetStats(new WeaponStats()
			{
				TimeToLive = 0.5f,
				Damage = WeaponStats.GetDamage() * 0.25f,
				Scale = WeaponStats.GetScale()
			});
		}

		private void OnTriggerEnter(Collider other)
		{
			if ((other.CompareTag("Enemy") || other.CompareTag("Destructible")) && BookWeapon.IsShadowBurst)
				SpawnExplosion();
			
			SimpleDamage(other, false);
		}
	}
}