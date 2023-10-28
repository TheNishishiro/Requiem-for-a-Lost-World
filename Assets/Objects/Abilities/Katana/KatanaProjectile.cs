using System;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Katana
{
	public class KatanaProjectile : PoolableProjectileWithLimitedHitBox<KatanaProjectile>
	{
		private void Awake()
		{
			GetComponent<ParticleSystem>().Play();
		}

		void Update()
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