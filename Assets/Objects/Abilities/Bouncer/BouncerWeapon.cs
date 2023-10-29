using System.Linq;
using DefaultNamespace;
using Managers;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Bouncer
{
	public class BouncerWeapon : PoolableWeapon<BouncerProjectile>
	{
		public float ElectroDefenceShred;
		public bool Thunderstorm;

		protected override bool ProjectileSpawn(BouncerProjectile projectile)
		{
			var currentPosition = transform.position;
			var target = FindObjectsByType<Damageable>(FindObjectsSortMode.None).OrderBy(_ => Random.value).FirstOrDefault();
			if (target is null)
				return false;

			projectile.transform.position = currentPosition;
			projectile.SetStats(weaponStats);
			projectile.SetTarget(target);
			return true;
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