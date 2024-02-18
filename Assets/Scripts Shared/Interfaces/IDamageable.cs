using Data.Elements;
using DefaultNamespace.Data.Weapons;
using UnityEngine;
using Weapons;

namespace Interfaces
{
	public interface IDamageable
	{
		void TakeDamage(float damage, WeaponBase weaponBase = null, bool isRecursion = false);
		void TakeDamage(DamageResult damage, WeaponBase damageSource, bool isRecursion = false);
		void SetHealth(float health);
		bool IsDestroyed();
		void ReduceElementalDefence(Element element, float amount);
		void TakeDamageWithCooldown(float damage, GameObject damageSource, float damageCooldown, WeaponBase weaponBase);
		void TakeDamageWithCooldown(DamageResult damage, GameObject damageSource, float damageCooldown, WeaponBase weaponBase);
		void SetVulnerable(float time, float percentage);
		void ApplyDamageOverTime(float damage, float damageFrequency, float damageDuration, WeaponBase weaponBase);
	}
}