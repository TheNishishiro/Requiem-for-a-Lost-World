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
			direction = (new Vector3(dirX, dirY, dirZ) - transform.position).normalized;
		}

		void Update()
		{
			TickLifeTime();
			transform.position += direction * ((WeaponStats?.GetSpeed() ?? 0) * Time.deltaTime);
		}

		private void OnTriggerEnter(Collider other)
		{
			SimpleDamage(other, true);
		}
		
		private void OnEnable()
		{
			trail.Clear();
			trail.emitting = true;
		}

		private void OnDisable()
		{
			trail.emitting = false;
		}
	}
}