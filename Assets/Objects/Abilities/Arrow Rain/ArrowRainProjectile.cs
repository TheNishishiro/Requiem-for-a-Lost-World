using System;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Arrow_Rain
{
	public class ArrowRainProjectile : ProjectileWithLimitedHitBoxBase
	{
		public void Update()
		{
			TickLifeTime();
			UpdateCollider();
		}
		
		private void OnTriggerEnter(Collider other)
		{
			SimpleDamage(other, false);
		}
	}
}