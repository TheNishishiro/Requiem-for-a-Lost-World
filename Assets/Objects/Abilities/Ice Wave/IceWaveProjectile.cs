﻿using System;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Ice_Wave
{
	public class IceWaveProjectile : PoolableProjectile<IceWaveProjectile>
	{
		private IceWaveWeapon IceWaveWeapon => ParentWeapon as IceWaveWeapon;

		private void OnTriggerEnter(Collider other)
		{
			SimpleDamage(other, false);
			if (!IceWaveWeapon.IsBlockingEnemies)
				return;
			
			var chaseComponent = other.GetComponentInParent<ChaseComponent>();
			if (chaseComponent != null)
			{
				chaseComponent.SetImmobile(0.2f);
			}
		}
	}
}