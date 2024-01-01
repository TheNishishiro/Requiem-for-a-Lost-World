using DefaultNamespace;
using Interfaces;
using Objects.Abilities;
using UnityEngine;

namespace Weapons
{
    public class DamageSource : MonoBehaviour
    {
	    protected WeaponStats WeaponStats;
	    protected WeaponBase ParentWeapon;
	    protected float damageCooldown;
	    protected int currentPassedEnemies;
	    protected bool isDamageCooldownExpired;
	    protected float ProjectileDamageIncreasePercentage;
	    protected bool IsDead;
	    
        public void TickDamageCooldown()
		{
			if (!isDamageCooldownExpired)
				damageCooldown -= Time.deltaTime;
			
			if (damageCooldown <= 0)
				isDamageCooldownExpired = true;
		}

		protected void ResetDamageCooldown()
		{
			if (WeaponStats is null) return;
			
			isDamageCooldownExpired = false;
			damageCooldown = WeaponStats.DamageCooldown;
		}

		protected void SimpleDamage(Collider other, bool isLimitedUsage)
		{
			SimpleDamage(other, isLimitedUsage, out _);
		}

		protected void SimpleDamage(Collider other, bool isLimitedUsage, out IDamageable damageable)
		{
			damageable = null;
			if (!other.CompareTag("Enemy") && !other.CompareTag("Destructible"))
				return;
			
			damageable = other.GetComponent<IDamageable>();
			if (damageable == null)
				return;

			if (damageable.IsDestroyed())
				return;

			damageable.TakeDamage(WeaponStats.GetDamage() * (1.0f + ProjectileDamageIncreasePercentage), ParentWeapon);

			if (isLimitedUsage && currentPassedEnemies-- <= 0)
				OnLifeTimeEnd();
		}

		protected void SimpleDamage(Damageable damageable, bool isLimitedUsage)
		{
			if (damageable == null)
				return;

			damageable.TakeDamage(WeaponStats.GetDamage() * (1.0f + ProjectileDamageIncreasePercentage), ParentWeapon);
			if (damageable.IsDestroyed())
				return;
			
			if (isLimitedUsage && currentPassedEnemies-- <= 0)
				OnLifeTimeEnd();
		}

		protected void DamageArea(Collider other, out IDamageable damageable)
		{
			damageable = null;
			if (!other.CompareTag("Enemy") && !other.CompareTag("Destructible"))
				return;
			
			damageable = other.GetComponent<IDamageable>();
			damageable?.TakeDamageWithCooldown(WeaponStats.GetDamage() * (1.0f + ProjectileDamageIncreasePercentage), gameObject, WeaponStats.DamageCooldown, ParentWeapon);
		}

		protected void DamageOverTime(Collider other)
		{
			if (!other.CompareTag("Enemy") && !other.CompareTag("Destructible"))
				return;
			
			var damageable = other.GetComponent<IDamageable>();
			
			if (!damageable.IsDestroyed())
				DamageOverTime(damageable, other);
		}

		protected void DamageOverTime(IDamageable damageable, Collider other)
		{
			if (!other.CompareTag("Enemy") && !other.CompareTag("Destructible"))
				return;

			if (!damageable.IsDestroyed())
			{
				damageable.ApplyDamageOverTime(WeaponStats.GetDamageOverTime(), WeaponStats.GetDamageOverTimeFrequency(), WeaponStats.GetDamageOverTimeDuration(), ParentWeapon);
			}
		}

		protected virtual void OnLifeTimeEnd()
		{
			IsDead = true;
			Destroy();
		}

		protected virtual void Destroy()
		{
			Destroy(gameObject);
		}
    }
}