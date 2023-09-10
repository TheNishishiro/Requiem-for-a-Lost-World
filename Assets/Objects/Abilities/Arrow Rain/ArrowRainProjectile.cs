using System;
using Data.Elements;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Arrow_Rain
{
	public class ArrowRainProjectile : ProjectileWithLimitedHitBoxBase
	{
		private ArrowRainWeapon ArrowRainWeapon => (ArrowRainWeapon) ParentWeapon;
		
		public void Update()
		{
			TickLifeTime();
			UpdateCollider();
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