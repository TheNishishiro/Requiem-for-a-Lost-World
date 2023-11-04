using System;
using Data.Elements;
using Interfaces;
using UnityEngine;
using UnityEngine.Pool;
using Weapons;

namespace Objects.Abilities.Arrow_Rain
{
	public class ArrowRainProjectile : PoolableProjectileWithLimitedHitBox<ArrowRainProjectile>
	{
		private ArrowRainWeapon ArrowRainWeapon => (ArrowRainWeapon) ParentWeapon;
		
		public void Update()
		{
			UpdateCollider();
			TickLifeTime();
		}
		
		private void OnTriggerEnter(Collider other)
		{
			SimpleDamage(other, false, out var target);

			if (target == null)
				return;
			
			if (ArrowRainWeapon.HailOfArrows)
				target.ReduceElementalDefence(Element.Physical, 0.005f);
		}
	}
}