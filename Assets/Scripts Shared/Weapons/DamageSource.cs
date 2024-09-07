using DefaultNamespace;
using Interfaces;
using Objects;
using Objects.Abilities;
using Unity.Netcode;
using UnityEngine;

namespace Weapons
{
    public class DamageSource : MonoBehaviour
    {
	    protected IWeaponStatsStrategy WeaponStatsStrategy;
	    protected IWeapon ParentWeapon;
	    protected float damageCooldown;
	    protected int currentPassedEnemies;
	    protected bool isDamageCooldownExpired;
	    protected float ProjectileDamageIncreasePercentage;
	    protected float ProjectileDamageIncrease;
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
			isDamageCooldownExpired = false;
			damageCooldown = WeaponStatsStrategy.GetDamageCooldown();
		}

		protected void SimpleDamage(Collider other, bool isLimitedUsage)
		{
			SimpleDamage(other, isLimitedUsage, false, out _);
		}

		protected void SimpleFollowUpDamage(Collider other)
		{
			SimpleDamage(other, false, true, out _);
		}

		protected void SimpleFollowUpDamage(IDamageable damageable)
		{
			SimpleDamage(damageable, false, true);
		}

		protected void SimpleDamage(Collider other, bool isLimitedUsage, bool isFollowUp, out IDamageable damageable)
		{
			damageable = null;
			if (!other.CompareTag("Enemy") && !other.CompareTag("Destructible"))
				return;
			
			damageable = other.GetComponent<IDamageable>();
			SimpleDamage(damageable, isLimitedUsage, isFollowUp);
		}

		protected void SimpleDamage(IDamageable damageable, bool isLimitedUsage, bool isFollowUp)
		{
			if (damageable == null)
				return;
			if (damageable.IsDestroyed())
				return;

			var damage = WeaponStatsStrategy.GetDamageDealt(ProjectileDamageIncreasePercentage, ProjectileDamageIncrease, isFollowUp);
			damageable.TakeDamage(damage, (WeaponBase)ParentWeapon, isFollowUp);

			if (isLimitedUsage && currentPassedEnemies-- <= 0)
				OnLifeTimeEnd();
		}

		protected void DamageArea(Collider other, out IDamageable damageable, bool isFollowUp = false)
		{
			damageable = null;
			if (!other.CompareTag("Enemy") && !other.CompareTag("Destructible"))
				return;
			
			damageable = other.GetComponent<IDamageable>();
			var damage = WeaponStatsStrategy.GetDamageDealt(ProjectileDamageIncreasePercentage, ProjectileDamageIncrease);
			damageable?.TakeDamageWithCooldown(damage, gameObject, WeaponStatsStrategy.GetDamageCooldown(), ParentWeapon, isFollowUp);
		}

		protected void DamageOverTime(Collider other, bool isLimitedUsage = false)
		{
			if (!other.CompareTag("Enemy") && !other.CompareTag("Destructible"))
				return;
			
			var damageable = other.GetComponent<IDamageable>();

			if (!damageable.IsDestroyed())
				DamageOverTime(damageable, other);
			
			if (isLimitedUsage && currentPassedEnemies-- <= 0)
				OnLifeTimeEnd();
		}

		protected void DamageOverTime(IDamageable damageable, Collider other)
		{
			if (!other.CompareTag("Enemy") && !other.CompareTag("Destructible"))
				return;

			if (!damageable.IsDestroyed())
			{
				var dot = WeaponStatsStrategy.GetDamageOverTime();
				var dotFrequency = WeaponStatsStrategy.GetDamageOverTimeFrequency();
				var dotDuration = WeaponStatsStrategy.GetDamageOverTimeDuration();
				
				damageable.ApplyDamageOverTime(dot, dotFrequency, dotDuration, ParentWeapon);
			}
		}

		protected void ApplyVulnerability(IDamageable damageable, Collider other, float duration)
		{
			if (!other.CompareTag("Enemy"))
				return;
			
			var weakness = WeaponStatsStrategy.GetWeakness();
			if (weakness > 0 && duration > 0)
			{
				damageable.SetVulnerable((WeaponEnum)ParentWeapon.GetId(), duration, weakness);
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