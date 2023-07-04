using DefaultNamespace;
using Interfaces;
using Objects.Abilities;
using UnityEngine;

namespace Weapons
{
	public class ProjectileBase : MonoBehaviour
	{
		protected WeaponStats WeaponStats;
		protected WeaponBase ParentWeapon;
		protected float TimeToLive;
		protected float TimeAlive;
		protected float DamageCooldown;
		protected float ProjectileDamageIncreasePercentage;
		protected bool isDamageCooldownExpired;
		protected int _currentPassedEnemies;
		protected bool _isDead;
		
		public virtual void SetParentWeapon(WeaponBase parentWeapon)
		{
			ParentWeapon = parentWeapon;
		}
		
		public virtual void SetStats(WeaponStats weaponStats)
		{
			WeaponStats = weaponStats;
			transform.localScale *= WeaponStats.GetScale();
			TimeToLive = GetTimeToLive();
			DamageCooldown = weaponStats.DamageCooldown;
			_currentPassedEnemies = weaponStats.GetPassThroughCount();
		}

		protected virtual float GetTimeToLive()
		{
			return WeaponStats.GetTimeToLive();
		}

		public void TickProjectile()
		{
			TickLifeTime();
			TickDamageCooldown();
		}

		public void TickLifeTime()
		{
			TimeToLive -= Time.deltaTime;
			TimeAlive += Time.deltaTime;

			if (TimeToLive <= 0 && !_isDead)
				OnLifeTimeEnd();
		}

		protected virtual void OnLifeTimeEnd()
		{
			_isDead = true;
			Destroy(gameObject);
		}

		public void TickDamageCooldown()
		{
			if (!isDamageCooldownExpired)
				DamageCooldown -= Time.deltaTime;
			
			if (DamageCooldown <= 0)
				isDamageCooldownExpired = true;
		}

		protected void ResetDamageCooldown()
		{
			if (WeaponStats is null) return;
			
			isDamageCooldownExpired = false;
			DamageCooldown = WeaponStats.DamageCooldown;
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

			damageable.TakeDamage(WeaponStats.GetDamage() * (1.0f + ProjectileDamageIncreasePercentage), ParentWeapon);
			
			if (isLimitedUsage && _currentPassedEnemies-- <= 0)
				Destroy(gameObject);
		}

		protected void SimpleDamage(Damageable damageable, bool isLimitedUsage)
		{
			if (damageable == null)
				return;

			damageable.TakeDamage(WeaponStats.GetDamage() * (1.0f + ProjectileDamageIncreasePercentage), ParentWeapon);
			
			if (isLimitedUsage && _currentPassedEnemies-- <= 0)
				Destroy(gameObject);
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
			DamageOverTime(damageable, other);
		}

		protected void DamageOverTime(IDamageable damageable, Collider other)
		{
			if (!other.CompareTag("Enemy") && !other.CompareTag("Destructible"))
				return;
			
			damageable.ApplyDamageOverTime(WeaponStats.GetDamageOverTime(), WeaponStats.DamageOverTimeFrequency, WeaponStats.DamageOverTimeDuration, ParentWeapon);
		}
	}
}