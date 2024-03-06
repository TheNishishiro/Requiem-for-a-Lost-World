using System;
using Interfaces;
using UnityEngine;
using UnityEngine.Pool;
using Weapons;

namespace Objects.Abilities.Magic_Ball
{
	public class FireBallProjectile : PoolableProjectile<FireBallProjectile>
	{
		private Vector3 _direction;
		private ObjectPool<FireBallProjectile> _objectPool;

		public void SetDirection(float dirX, float dirY, float dirZ)
		{
			_direction = (new Vector3(dirX, dirY, dirZ) - transformCache.position).normalized;
		}

		protected override void CustomUpdate()
		{
			transformCache.position += _direction * ((WeaponStatsStrategy?.GetSpeed() ?? 0) * Time.deltaTime);
		}

		private void OnTriggerEnter(Collider other)
		{
			SimpleDamage(other, true);
		}
	}
}