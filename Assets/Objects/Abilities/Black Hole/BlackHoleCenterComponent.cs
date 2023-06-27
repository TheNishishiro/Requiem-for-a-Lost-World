using System;
using Interfaces;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Back_Hole
{
	public class BlackHoleCenterComponent : ProjectileBase
	{
		private void OnTriggerStay(Collider other)
		{
			DamageArea(other, out var damageable);
			damageable?.SetVulnerable(1f, WeaponStats.GetWeakness());
		}
	}
}