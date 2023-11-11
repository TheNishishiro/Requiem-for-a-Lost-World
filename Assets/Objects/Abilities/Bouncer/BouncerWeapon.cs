using System.Linq;
using DefaultNamespace;
using Managers;
using UnityEngine;
using UnityEngine.Pool;
using Weapons;

namespace Objects.Abilities.Bouncer
{
	public class BouncerWeapon : PoolableWeapon<BouncerProjectile>
	{
		public float ElectroDefenceShred;
		public bool Thunderstorm;

		private ObjectPool<BouncerProjectile> _subProjectilePool;
		private Vector3 _subProjectilePosition;

		public override void Awake()
		{
			base.Awake();

			var subProjectileStats = new WeaponStats()
			{
				TimeToLive = 0.5f,
				Scale = 0.5f,
				Speed = 1,
				PassThroughCount = 1
			};
			_subProjectilePool = new ObjectPool<BouncerProjectile>(
				() =>
				{
					var projectile = SpawnManager.instance.SpawnObject(_subProjectilePosition, spawnPrefab).GetComponent<BouncerProjectile>();
					projectile.Init(_subProjectilePool, projectile);
					projectile.IsSubSpawned = true;
					return projectile;
				},
				projectile =>
				{
					subProjectileStats.Damage = weaponStats.GetDamage() * 0.5f;
					projectile.SetStats(subProjectileStats);
					projectile.SetParentWeapon(this);
					projectile.FindNextTarget();
					projectile.gameObject.SetActive(true);
				},
				projectile => projectile.gameObject.SetActive(false),
				projectile => Destroy(projectile.gameObject),
				true, 150, 200
			);
		}
		
		public void SpawnSubProjectile(Vector3 position)
		{
			_subProjectilePosition = position;
			_subProjectilePool.Get();
		}
		
		protected override bool ProjectileSpawn(BouncerProjectile projectile)
		{
			var currentPosition = transform.position;
			var target = EnemyManager.instance.GetRandomEnemy().GetDamagableComponent();
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
				case 4:
					ElectroDefenceShred += 0.2f;
					break;
				case 8:
					ElectroDefenceShred += 0.1f;
					break;
				case 11:
					Thunderstorm = true;
					break;
			}
		}
	}
}