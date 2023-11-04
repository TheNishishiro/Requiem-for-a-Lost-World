using DefaultNamespace;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Managers;
using Objects.Abilities.BindingField;
using UnityEngine;
using UnityEngine.Pool;
using Weapons;

namespace Objects.Abilities.Healing_Field
{
	public class HealingFieldWeapon : PoolableWeapon<HealingFieldProjectile>
	{
		[SerializeField] private GameObject healingFieldPrefab;
		[HideInInspector] public bool IsEmpowering;
		private ObjectPool<HealingField> _subProjectilePool;
		private Vector3 _subProjectilePosition;
		
		public override void Awake()
		{
			base.Awake();

			var subProjectileStats = new WeaponStats()
			{
				TimeToLive = 1f
			};
			_subProjectilePool = new ObjectPool<HealingField>(
				() =>
				{
					var projectile = SpawnManager.instance.SpawnObject(_subProjectilePosition, healingFieldPrefab).GetComponent<HealingField>();
					projectile.Init(_subProjectilePool, projectile);
					return projectile;
				},
				projectile =>
				{
					subProjectileStats.Scale = weaponStats.GetScale();
					projectile.SetStats(subProjectileStats);
					projectile.Setup(weaponStats.HealPerHit, IsEmpowering);
					projectile.gameObject.SetActive(true);
				},
				projectile => projectile.gameObject.SetActive(false),
				projectile => Destroy(projectile.gameObject),
				true, 20, 40
			);
		}
		
		public void SpawnSubProjectile(Vector3 position)
		{
			_subProjectilePosition = position;
			_subProjectilePool.Get();
		}
		
		protected override bool ProjectileSpawn(HealingFieldProjectile projectile)
		{
			var pointOnSurface = Utilities.GetPointOnColliderSurface(new Vector3(transform.position.x, 0, transform.position.z), transform);
			projectile.transform.position = pointOnSurface;
			projectile.SetStats(weaponStats);
			return true;
		}

		protected override void OnLevelUp()
		{
			if (LevelField == 9)
				IsEmpowering = true;
		}

		protected override int GetAttackCount()
		{
			return 1;
		}
	}
}