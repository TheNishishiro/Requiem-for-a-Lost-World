using System;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Meteor
{
	public class MeteorProjectile : PoolableProjectile<MeteorProjectile>
	{
		private Vector3 direction;
		private float destroyY;
		
		public void SetDirection(float dirX, float dirY, float dirZ)
		{
			direction = (new Vector3(dirX, dirY, dirZ) - transform.position).normalized;
			destroyY = dirY - 5.0f;
		}

		void LateUpdate()
		{
			transform.position += direction * ((WeaponStats?.GetSpeed()).GetValueOrDefault() * Time.deltaTime);
			
			if (destroyY != 0 && transform.localPosition.y < destroyY)
				Destroy(gameObject);
		}
		
		private void OnTriggerEnter(Collider other)
		{
			SimpleDamage(other, false);
		}
	}
}