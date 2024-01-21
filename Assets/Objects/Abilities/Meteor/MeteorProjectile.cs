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
			direction = (new Vector3(dirX, dirY, dirZ) - transformCache.position).normalized;
			destroyY = dirY - 5.0f;
		}

		void LateUpdate()
		{
			transformCache.position += direction * ((WeaponStatsStrategy?.GetSpeed()).GetValueOrDefault() * Time.deltaTime);
			
			if (transformCache.localPosition.y < destroyY)
				Destroy();
		}
		
		private void OnTriggerEnter(Collider other)
		{
			SimpleDamage(other, false);
		}
	}
}