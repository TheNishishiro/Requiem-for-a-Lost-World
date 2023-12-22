using System;
using DefaultNamespace;
using DefaultNamespace.Data.VFX_Stages;
using Interfaces;
using NaughtyAttributes;
using Objects.Abilities;
using UnityEngine;
using UnityEngine.Pool;

namespace Weapons
{
	public class ProjectileBase : DamageSource
	{
		private Vector3 baseScale;
		protected float TimeToLive;
		protected float TimeAlive;
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
			damageCooldown = weaponStats.DamageCooldown;
			currentPassedEnemies = weaponStats.GetPassThroughCount();
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

		protected void ReturnToPool<T>(ObjectPool<T> pool, T entity) where T : MonoBehaviour
		{
			pool.Release(entity);
		}
	}
}