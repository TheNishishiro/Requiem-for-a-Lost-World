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
				
			direction = (target.transform.position - transform.position).normalized;
		}

		void Update()
		{
			TickLifeTime();
			transform.position += direction * ((WeaponStats?.GetSpeed() ?? 0) * Time.deltaTime);
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
			{
				var bouncer = SpawnManager.instance.SpawnObject(transform.position, ParentWeapon.spawnPrefab);
				var projectileComponent = bouncer.GetComponent<BouncerProjectile>();
				projectileComponent.IsSubSpawned = true;
				projectileComponent.SetStats(new WeaponStats()
				{
					Damage = WeaponStats.Damage * 0.5f,
					TimeToLive = 0.5f,
					Scale = 0.5f,
					Speed = 1,
					PassThroughCount = 1
				});
				projectileComponent.SetParentWeapon(ParentWeapon);
				projectileComponent.FindNextTarget();
			}

			yield return null;
		}
	}
}