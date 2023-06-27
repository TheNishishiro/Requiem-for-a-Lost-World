using System;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Katana
{
	public class KatanaProjectile : ProjectileWithLimitedHitBoxBase
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