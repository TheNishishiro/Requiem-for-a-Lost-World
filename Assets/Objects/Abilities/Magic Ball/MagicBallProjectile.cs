using System;
using Interfaces;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Magic_Ball
{
	public class MagicBallProjectile : ProjectileBase
	{
		private Vector3 direction;

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
	}
}