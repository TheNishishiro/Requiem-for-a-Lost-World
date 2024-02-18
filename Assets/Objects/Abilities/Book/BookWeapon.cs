using System.Collections;
using DefaultNamespace;
using Managers;
using Objects.Abilities.Magic_Ball;
using Objects.Abilities.SpaceExpansionBall;
using UnityEngine;
using UnityEngine.Pool;
using Weapons;

namespace Objects.Abilities.Book
{
	public class BookWeapon : PoolableWeapon<BookProjectile>
	{
		private float rotateOffset = 0;
		[HideInInspector] public bool IsShadowBurst;
		[SerializeField] public GameObject ExplosionPrefab;
		private ObjectPool<SimpleDamageProjectile> _subProjectilePool;
		private Vector3 _subProjectilePosition;
		private WeaponStats _subProjectileStats;
		private WeaponStatsStrategyBase _subProjectileStatsStrategy;

		public override void Awake()
		{
			base.Awake();

			_subProjectileStats = new WeaponStats()
			{
				TimeToLive = 0.5f
			};
			_subProjectileStatsStrategy = new WeaponStatsStrategyBase(_subProjectileStats, ElementField);
			
			_subProjectilePool = new ObjectPool<SimpleDamageProjectile>(
				() =>
				{
					var projectile = SpawnManager.instance.SpawnObject(_subProjectilePosition, ExplosionPrefab)
						.GetComponent<SimpleDamageProjectile>();
					projectile.Init(_subProjectilePool, projectile);
					return projectile;
				},
				projectile =>
				{
					projectile.SetParentWeapon(this, false);
					_subProjectileStats.Damage = WeaponStatsStrategy.GetDamage() * 0.25f;
					_subProjectileStats.Scale = WeaponStatsStrategy.GetScale();
					projectile.SetStats(_subProjectileStatsStrategy);
					projectile.transform.position = _subProjectilePosition;
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

		protected override BookProjectile ProjectileInit()
		{
			var book = Instantiate(spawnPrefab, transform).GetComponent<BookProjectile>();
			book.SetParentWeapon(this);
			book.SetTarget(gameObject);
			return book;
		}

		protected override bool ProjectileSpawn(BookProjectile projectile)
		{
			projectile.transform.position = transform.position + new Vector3(WeaponStatsStrategy.GetScale(),0,0);
			projectile.transform.RotateAround(transform.position, Vector3.up, rotateOffset);
			projectile.SetParentWeapon(this);
			return true;
		}

		protected override IEnumerator AttackProcess()
		{
			var rotationStep = GetRotationByAttackCount();
			rotateOffset = 0;
			
			for (var i = 0; i < WeaponStatsStrategy.GetAttackCount(); i++)
			{
				Attack();
				rotateOffset += rotationStep;
			}

			yield break;
		}

		protected override void OnLevelUp()
		{
			if (LevelField == 10)
				IsShadowBurst = true;
		}
	}
}