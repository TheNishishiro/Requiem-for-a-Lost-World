using System;
using DefaultNamespace;
using Objects.Characters;
using Objects.Stage;
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
				if (BindingFieldWeapon.IsBurstDamage && TimeAlive <= 0.25f)
				{
					ProjectileDamageIncreasePercentage = 20;
					SimpleDamage(other, false);
					ProjectileDamageIncreasePercentage = 0;
				}

				damageable?.SetVulnerable(CurrentTimeToLive, WeaponStatsStrategy.GetWeakness());
				chaseComponent.SetImmobile(CurrentTimeToLive);
				if (GameData.IsCharacterWithRank(CharactersEnum.Amelisana_BoN, CharacterRank.E3))
				{
					chaseComponent.SetSlow(CurrentTimeToLive, 0.3f);
				}
			}
		}

		private void OnTriggerStay(Collider other)
		{
			DamageArea(other, out var damageable);
		}
	}
}