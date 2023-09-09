using System.Linq;
using DefaultNamespace;
using Managers;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Bouncer
{
	public class BouncerWeapon : WeaponBase
	{
		public override void Attack()
		{
			var currentPosition = transform.position;
			var target = FindObjectsOfType<Damageable>().OrderBy(_ => Random.value).FirstOrDefault();
			if (target is null)
				return;

			var bouncer = SpawnManager.instance.SpawnObject(currentPosition, spawnPrefab);
			var projectileComponent = bouncer.GetComponent<BouncerProjectile>();
			projectileComponent.SetStats(weaponStats);
			projectileComponent.SetParentWeapon(this);
			projectileComponent.SetTarget(target);
		}
	}
}