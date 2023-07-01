using Managers;
using Objects.Abilities.Lightning_Chain;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.LightningStrike
{
	public class LightningStrikeProjectile : ProjectileBase
	{
		private LightningStrikeWeapon _lightningStrikeWeapon => ParentWeapon as LightningStrikeWeapon;
		
		void Update()
		{
			TickLifeTime();
		}
		
		private void OnTriggerEnter(Collider other)
		{
			SimpleDamage(other, false, out var damageable);
			if (damageable != null && _lightningStrikeWeapon.IsChainLightning)
				SpawnChainLightning(other);
		}

		private void SpawnChainLightning(Component other)
		{
			var chainLighting = SpawnManager.instance.SpawnObject(other.gameObject.transform.position, _lightningStrikeWeapon.chainLightningPrefab);
			var lightingChainProjectile = chainLighting.GetComponent<LightningChainProjectile>();
			lightingChainProjectile.SetParentWeapon(ParentWeapon);
			lightingChainProjectile.SetStats(new WeaponStats()
			{
				TimeToLive = 0.5f,
				Damage = WeaponStats.GetDamage() * 0.25f,
				Scale = WeaponStats.GetScale(),
				DetectionRange = 1f
			});
			lightingChainProjectile.SeekTargets(5);
		}
	}
}