using System;
using DefaultNamespace;
using Interfaces;
using NaughtyAttributes;
using Objects.Abilities;
using UnityEngine;
using UnityEngine.Pool;

namespace Weapons
{
	public class ProjectileBase : MonoBehaviour
	{
		private Vector3 baseScale;
		protected WeaponStats WeaponStats;
		protected WeaponBase ParentWeapon;
		protected float TimeToLive;
		protected float TimeAlive;
		private float _damageCooldown;
		protected float ProjectileDamageIncreasePercentage;
		protected bool isDamageCooldownExpired;
		private int _currentPassedEnemies;
		protected bool IsDead;
		protected Transform transformCache;
		[SerializeField] public bool UseParticles;
		[ShowIf("UseParticles")]
		[SerializeField] public ParticleSystem ParticleSystem;

		protected virtual void Awake()
		{
			transformCache = transform;
			var localScale = transformCache.localScale;
			baseScale = new Vector3(localScale.x,localScale.y,localScale.z);
		}

		public virtual void SetParentWeapon(WeaponBase parentWeapon)
		{
			ParentWeapon = parentWeapon;
		}
		
		public virtual void SetStats(WeaponStats weaponStats)
		{
			WeaponStats = weaponStats;
			transform.localScale = baseScale * WeaponStats.GetScale();
			TimeToLive = GetTimeToLive();
			_damageCooldown = weaponStats.DamageCooldown;
			_currentPassedEnemies = weaponStats.GetPassThroughCount();
			StopAllCoroutines();
			IsDead = false;
			TimeAlive = 0;
			isDamageCooldownExpired = false;
			ProjectileDamageIncreasePercentage = 0;
			if (UseParticles)
			{
				ParticleSystem.Simulate( 0.0f, true, true );
				ParticleSystem.Play();
			}
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

			if (TimeToLive <= 0 && !IsDead)
				OnLifeTimeEnd();
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

		protected void ReturnToPool<T>(ObjectPool<T> pool, T entity) where T : MonoBehaviour
		{
			pool.Release(entity);
		}

		public void TickDamageCooldown()
		{
			if (!isDamageCooldownExpired)
				_damageCooldown -= Time.deltaTime;
			
			if (_damageCooldown <= 0)
				isDamageCooldownExpired = true;
		}

		protected void ResetDamageCooldown()
		{
			if (WeaponStats is null) return;
			
			isDamageCooldownExpired = false;
			_damageCooldown = WeaponStats.DamageCooldown;
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

			if (isLimitedUsage && _currentPassedEnemies-- <= 0)
				OnLifeTimeEnd();
		}

		protected void SimpleDamage(Damageable damageable, bool isLimitedUsage)
		{
			if (damageable == null)
				return;

			damageable.TakeDamage(WeaponStats.GetDamage() * (1.0f + ProjectileDamageIncreasePercentage), ParentWeapon);
			if (damageable.IsDestroyed())
				return;
			
			if (isLimitedUsage && _currentPassedEnemies-- <= 0)
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
				damageable.ApplyDamageOverTime(WeaponStats.GetDamageOverTime(), WeaponStats.GetDamageOverTimeFrequency(), WeaponStats.GetDamageOverTimeDuration(), ParentWeapon);
		}
	}
}