using System;
using Interfaces;
using Objects.Characters;
using Objects.Enemies;
using Objects.Players.Scripts;
using Objects.Stage;
using UnityEngine;
using Weapons;
using Random = UnityEngine.Random;

namespace Objects.Abilities.Scythe
{
	public class ScytheProjectile : PoolableProjectile<ScytheProjectile>
	{
		private HealthComponent _healthComponent;
		private ScytheWeapon ScytheWeapon => (ScytheWeapon) ParentWeapon;

		protected override float GetTimeToLive()
		{
			return 1.4f;
		}

		public void OnTriggerEnter(Collider other)
		{
			if (!other.CompareTag("Enemy") && !other.CompareTag("Destructible"))
				return;
			
			SimpleDamage(other, false, false, out var damageable);
			_healthComponent.Damage(-WeaponStatsStrategy.GetHealPerHit(GameData.IsCharacterWithRank(CharactersEnum.Corina_BoB, CharacterRank.E1)));

			if (ScytheWeapon.IsBloodEmbrace && Random.value <= 0.2)
				DamageOverTime(damageable, other);
			if (ScytheWeapon.IsCursedStrikes && Random.value <= 0.1 && damageable != null)
				damageable.SetVulnerable((WeaponEnum)ParentWeapon.GetId(), 2, 1);
			if (ScytheWeapon.IsSoulHarvest && Random.value <= 0.1)
				_healthComponent.IncreaseMaxHealth(0.001f);
		}

		public void SetPlayerHealthComponent(HealthComponent healthComponent)
		{
			_healthComponent = healthComponent;
		}
	}
}