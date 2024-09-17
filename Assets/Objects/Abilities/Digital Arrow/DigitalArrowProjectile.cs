using System;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Magic_Ball
{
	public class DigitalArrowProjectile : PoolableProjectile<DigitalArrowProjectile>
	{
		private DigitalArrowWeapon DigitalArrowWeapon => (DigitalArrowWeapon)ParentWeapon;
		private Vector3 direction;
		[SerializeField] private List<TrailRenderer> trails;
		[SerializeField] private ParticleSystem particlesOnHit;
		
		public void SetDirection(float dirX, float dirY, float dirZ)
		{
			direction = (new Vector3(dirX, dirY, dirZ) - transformCache.position).normalized;
			transformCache.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90,0,0);
		}

		public override void SetParentWeapon(IWeapon parentWeapon, bool initStats = true)
		{
			base.SetParentWeapon(parentWeapon, initStats);
			ProjectileDamageIncreasePercentage = 0;
		}

		protected override void CustomUpdate()
		{
			transformCache.position += direction * ((WeaponStatsStrategy?.GetSpeed() ?? 0) * Time.deltaTime);
		}

		private void OnTriggerEnter(Collider other)
		{
			particlesOnHit.Simulate( 0.0f, true, true );
			particlesOnHit.Play();
			SplitDamage(other, true);
			if (DigitalArrowWeapon.IsAlgorithmicCascade)
				ProjectileDamageIncreasePercentage += 0.2f;
		}
		
		public void ClearTrail()
		{
			trails.ForEach(x => x.Clear());
		}
		
		private void OnEnable()
		{
			trails.ForEach(x => x.emitting = true);
		}

		private void OnDisable()
		{
			trails.ForEach(x => x.emitting = false);
			ClearTrail();
		}
	}
}