using System;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Throwing_Knife
{
	public class ThrowingKnifeProjectile : PoolableProjectile<ThrowingKnifeProjectile>
	{
		private Vector3 direction;
		
		protected override void CustomUpdate()
		{
			transform.position += direction * (WeaponStatsStrategy.GetSpeed() * Time.deltaTime);
		}
		
		public void SetDirection(Vector3 dir)
		{
			direction = dir;
			transform.rotation = Quaternion.LookRotation(direction.normalized) * Quaternion.Euler(90,0,0);
		}

		private void OnTriggerEnter(Collider other)
		{
			SimpleDamage(other, true);
		}
	}
}