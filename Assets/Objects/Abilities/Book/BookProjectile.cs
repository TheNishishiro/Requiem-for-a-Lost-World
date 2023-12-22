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
		private float _explosionCooldown;

		public void SetTarget(GameObject rotateTarget)
		{
			this.rotateTarget = rotateTarget;
		}
		
		protected override void CustomUpdate()
		{
			if (rotateTarget != null)
			{
				transform.RotateAround(rotateTarget.transform.position, Vector3.up, Time.deltaTime * WeaponStats.GetSpeed());
			}

			if (_explosionCooldown > 0)
				_explosionCooldown -= Time.deltaTime;
		}

		private void SpawnExplosion()
		{
			if (_explosionCooldown > 0)
				return;
			
			BookWeapon.SpawnSubProjectile(transformCache.position);
			_explosionCooldown = 0.5f;
		}

		private void OnTriggerEnter(Collider other)
		{
			if ((other.CompareTag("Enemy") || other.CompareTag("Destructible")) && BookWeapon.IsShadowBurst)
				SpawnExplosion();
			
			SimpleDamage(other, false);
		}
	}
}