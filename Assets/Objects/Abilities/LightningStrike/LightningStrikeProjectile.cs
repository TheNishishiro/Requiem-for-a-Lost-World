using Managers;
using Objects.Abilities.Lightning_Chain;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.LightningStrike
{
	public class LightningStrikeProjectile : PoolableProjectile<LightningStrikeProjectile>
	{
		private LightningStrikeWeapon _lightningStrikeWeapon => ParentWeapon as LightningStrikeWeapon;
		
		private void OnTriggerEnter(Collider other)
		{
			SimpleDamage(other, false, false, out var damageable);
			if (damageable != null && _lightningStrikeWeapon.IsChainLightning)
				SpawnChainLightning(other);
		}

		private void SpawnChainLightning(Component other)
		{
			_lightningStrikeWeapon.SpawnSubProjectile(other.gameObject.transform.position);
		}
	}
}