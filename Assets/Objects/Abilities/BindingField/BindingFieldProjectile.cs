using System;
using DefaultNamespace;
using UnityEngine;
using Weapons;
using Random = UnityEngine.Random;

namespace Objects.Abilities.BindingField
{
	public class BindingFieldProjectile : PoolableProjectile<BindingFieldProjectile>
	{
		private BindingFieldWeapon BindingFieldWeapon => ParentWeapon as BindingFieldWeapon;

		private void OnTriggerEnter(Collider other)
		{
			var chaseComponent = other.GetComponentInParent<ChaseComponent>();
			var damageable = other.GetComponent<Damageable>();
			
			if (chaseComponent != null && Random.value < BindingFieldWeapon.ChanceToBind)
			{
				if (BindingFieldWeapon.IsBurstDamage)
				{
					ProjectileDamageIncreasePercentage = 20;
					SimpleDamage(other, false);
					ProjectileDamageIncreasePercentage = 0;
				}

				damageable?.SetVulnerable(CurrentTimeToLive, WeaponStatsStrategy.GetWeakness());
				chaseComponent.SetImmobile(CurrentTimeToLive);
			}
		}

		private void OnTriggerStay(Collider other)
		{
			DamageArea(other, out _);
		}
	}
}