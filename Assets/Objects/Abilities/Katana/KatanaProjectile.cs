using System;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Katana
{
	public class KatanaProjectile : PoolableProjectile<KatanaProjectile>
	{
		// HBLT: 0.1
		
		private void OnTriggerEnter(Collider other)
		{
			SimpleDamage(other, false);
		}
	}
}