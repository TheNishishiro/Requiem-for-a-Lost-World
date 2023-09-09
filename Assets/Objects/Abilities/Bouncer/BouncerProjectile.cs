using System.Linq;
using DefaultNamespace;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Bouncer
{
	public class BouncerProjectile : ProjectileBase
	{
		private Vector3 direction;
		
		public void SetTarget(Damageable target)
		{
			if (target == null)
				OnLifeTimeEnd();
				
			direction = (target.transform.position - transform.position).normalized;
		}

		void Update()
		{
			TickLifeTime();
			transform.position += direction * ((WeaponStats?.GetSpeed() ?? 0) * Time.deltaTime);
		}

		private void OnTriggerEnter(Collider other)
		{
			SimpleDamage(other, true);
			FindNextTarget();
		}

		private void FindNextTarget()
		{
			var target = FindObjectsOfType<Damageable>().OrderBy(_ => Random.value).FirstOrDefault();
			SetTarget(target);
		}
	}
}