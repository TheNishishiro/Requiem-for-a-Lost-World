using System.Collections;
using System.Linq;
using Data.Elements;
using DefaultNamespace;
using Managers;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Bouncer
{
	public class BouncerProjectile : PoolableProjectile<BouncerProjectile>
	{
		private Vector3 direction;
		private BouncerWeapon BouncerWeapon => (BouncerWeapon) ParentWeapon;
		public bool IsSubSpawned;
		
		public void SetTarget(Damageable target)
		{
			if (target == null)
				OnLifeTimeEnd();
				
			direction = (target.targetPoint.transform.position - transformCache.position).normalized;
		}

		void Update()
		{
			TickLifeTime();
			transformCache.position += direction * ((WeaponStats?.GetSpeed() ?? 0) * Time.deltaTime);
		}

		private void OnTriggerEnter(Collider other)
		{
			SimpleDamage(other, true, out var damageable);
			FindNextTarget();

			if (damageable == null)
				return;
			
			if (BouncerWeapon.ElectroDefenceShred > 0)
				damageable.ReduceElementalDefence(Element.Lightning, BouncerWeapon.ElectroDefenceShred);
			if (BouncerWeapon.Thunderstorm && !IsSubSpawned)
			{
				StartCoroutine(SpawnThunderstorm());
			}
		}

		public void FindNextTarget()
		{
			var target = EnemyManager.instance.GetRandomEnemy().GetDamagableComponent();
			SetTarget(target);
		}
		
		private IEnumerator SpawnThunderstorm()
		{
			for (var i = 0; i < 3; i++)
				BouncerWeapon.SpawnSubProjectile(transformCache.position);

			yield return null;
		}
	}
}