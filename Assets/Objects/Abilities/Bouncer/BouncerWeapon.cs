using System.Linq;
using DefaultNamespace;
using Managers;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Bouncer
{
	public class BouncerWeapon : WeaponBase
	{
		public float ElectroDefenceShred;
		public bool Thunderstorm;
		
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

		protected override void OnLevelUp()
		{
			switch (LevelField)
			{
				case 2:
					ElectroDefenceShred += 0.2f;
					break;
				case 6:
					ElectroDefenceShred += 0.1f;
					break;
				case 9:
					Thunderstorm = true;
					break;
			}
		}
	}
}