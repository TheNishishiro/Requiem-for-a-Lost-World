using System;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.SpaceExpansionBall
{
	public class SimpleDamageProjectile : PoolableProjectileWithLimitedHitBox<SimpleDamageProjectile>
	{
		[SerializeField] private bool limitedTimeCollider;
		
		public void Update()
		{
			TickLifeTime();
			if (limitedTimeCollider)
				UpdateCollider();
		}

		private void OnTriggerEnter(Collider other)
		{
			SimpleDamage(other, false);
		}
	}
}