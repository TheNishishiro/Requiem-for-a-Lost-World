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
		protected float TotalTimeToLive;
		protected float TimeLeftToLive => TotalTimeToLive - TimeAlive;
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
			SetStats(ParentWeapon.WeaponStatsStrategy);
		}
		
		private void SetStats(IWeaponStatsStrategy weaponStatsStrategy)
		{
			WeaponStatsStrategy = weaponStatsStrategy;
			transform.localScale = baseScale * WeaponStatsStrategy.GetScale();
			TimeToLive = TotalTimeToLive = GetTimeToLive();
			damageCooldown = WeaponStatsStrategy.GetDamageCooldown();
			currentPassedEnemies = WeaponStatsStrategy.GetPassThroughCount();
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
			return WeaponStatsStrategy.GetTotalTimeToLive();
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