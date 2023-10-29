using System;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Katana
{
	public class KatanaProjectile : PoolableProjectileWithLimitedHitBox<KatanaProjectile>
	{
		void Update()
		{
			UpdateCollider();
			TickLifeTime();
		}
		
		private void OnTriggerEnter(Collider other)
		{
			SimpleDamage(other, false);
		}
	}
}