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
		private Vector3 offset;

		public void SetTarget(GameObject rotateTarget)
		{
			this.rotateTarget = rotateTarget;
			offset = transformCache.position - rotateTarget.transform.position;
		}
		
		protected override void CustomUpdate()
		{
			if (rotateTarget != null) {
				offset = Quaternion.AngleAxis(Time.deltaTime * WeaponStatsStrategy.GetSpeed(), Vector3.up) * offset;

				transformCache.position = rotateTarget.transform.position + offset;

				Vector3 projectileToPlayer = (rotateTarget.transform.position - transformCache.position).normalized;

				// Calculate the direction the projectile should face
				Vector3 faceDirection = Vector3.Cross(projectileToPlayer, Vector3.up);

				transformCache.rotation = Quaternion.LookRotation(faceDirection, Vector3.up);
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