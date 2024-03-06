using System;
using Interfaces;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Magic_Ball
{
	public class MagicBallProjectile : PoolableProjectile<MagicBallProjectile>
	{
		private Vector3 direction;
		[SerializeField] private TrailRenderer trail;
		
		public void SetDirection(float dirX, float dirY, float dirZ)
		{
			direction = (new Vector3(dirX, dirY, dirZ) - transformCache.position).normalized;
		}

		protected override void CustomUpdate()
		{
			transformCache.position += direction * ((WeaponStatsStrategy?.GetSpeed() ?? 0) * Time.deltaTime);
		}

		private void OnTriggerEnter(Collider other)
		{
			SimpleDamage(other, true);
		}
		
		public void ClearTrail()
		{
			trail.Clear();
		}
		
		private void OnEnable()
		{
			trail.emitting = true;
		}

		private void OnDisable()
		{
			trail.emitting = false;
			ClearTrail();
		}
	}
}