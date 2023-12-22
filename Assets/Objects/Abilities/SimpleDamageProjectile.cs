using System;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.SpaceExpansionBall
{
	public class SimpleDamageProjectile : PoolableProjectile<SimpleDamageProjectile>
	{
		private void OnTriggerEnter(Collider other)
		{
			SimpleDamage(other, false);
		}
	}
}