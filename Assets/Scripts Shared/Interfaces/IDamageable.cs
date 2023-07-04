using UnityEngine;
using Weapons;

namespace Interfaces
{
	public interface IDamageable
	{
		void TakeDamage(float damage, WeaponBase damageSource);
		void SetHealth(float health);
		bool IsDestroyed();
		void TakeDamageWithCooldown(float damage, GameObject damageSource, float damageCooldown, WeaponBase weaponBase);
		void SetVulnerable(float time, float percentage);
		void ApplyDamageOverTime(float damage, float damageFrequency, float damageDuration, WeaponBase weaponBase);
	}
}