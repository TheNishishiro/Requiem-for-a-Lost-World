using System;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Ice_Wave
{
	public class IceWaveProjectile : ProjectileWithLimitedHitBoxBase
	{
		private IceWaveWeapon IceWaveWeapon => ParentWeapon as IceWaveWeapon;
		
		private void Update()
		{
			TickLifeTime();
			UpdateCollider();
		}

		private void OnTriggerEnter(Collider other)
		{
			SimpleDamage(other, false);
		}

		protected override void OnColliderEnd()
		{
			if (IceWaveWeapon.IsBlockingEnemies)
				collider.isTrigger = false;
			else
				base.OnColliderEnd();
		}
	}
}