using System.Linq;
using DefaultNamespace;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Managers;
using Objects.Abilities.Lightning_Chain;
using Objects.Characters;
using Objects.Environment;
using Objects.Stage;
using UnityEngine;
using UnityEngine.Pool;
using Weapons;
using Random = UnityEngine.Random;

namespace Objects.Abilities.LightningStrike
{
	public class LightningStrikeWeapon : PoolableWeapon<LightningStrikeProjectile>
	{
		[SerializeField] public GameObject chainLightningPrefab;
		[HideInInspector] public bool IsChainLightning;
		
		private ObjectPool<LightningChainProjectile> _subProjectilePool;
		private Vector3 _subProjectilePosition;
		private int _chargeStacks;

		public override void Awake()
		{
			base.Awake();

			var subProjectileStats = new WeaponStats()
			{
				TimeToLive = 0.5f,
				DetectionRange = 1f
			};
			_subProjectilePool = new ObjectPool<LightningChainProjectile>(
				() =>
				{
					var projectile = SpawnManager.instance.SpawnObject(_subProjectilePosition, chainLightningPrefab).GetComponent<LightningChainProjectile>();
					projectile.Init(_subProjectilePool, projectile);
					projectile.SetParentWeapon(this);
					return projectile;
				},
				projectile =>
				{
					subProjectileStats.Damage = WeaponStatsStrategy.GetDamage() * (GameData.IsCharacterWithRank(CharactersEnum.Alice_BoL, CharacterRank.E4) ? 0.75f : 0.25f);
					subProjectileStats.Scale = WeaponStatsStrategy.GetScale();
					projectile.SetParentWeapon(this);
					projectile.gameObject.SetActive(true);
					projectile.SeekTargets(5);
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

		protected override bool ProjectileSpawn(LightningStrikeProjectile projectile)
		{
			var enemy = EnemyManager.instance.GetRandomEnemy();
			if (enemy == null) return false;

			projectile.transform.position = enemy.transform.position;
			projectile.SetParentWeapon(this);
			return true;
		}

		protected override int GetAttackCount()
		{
			var attackCount = base.GetAttackCount();
			if (GameData.IsCharacterWithRank(CharactersEnum.Alice_BoL, CharacterRank.E1))
				attackCount += 4;
			
			if (GameData.IsCharacterWithRank(CharactersEnum.Alice_BoL, CharacterRank.E5))
				attackCount += _chargeStacks/2;

			_chargeStacks = 0;
			return attackCount;
		}

		protected override void OnLevelUp()
		{
			if (LevelField == 8)
				IsChainLightning = true;
		}

		public override void OnEnemyKilled()
		{
			base.OnEnemyKilled();
			if (Random.value < 0.6f) _chargeStacks++;
		}
	}
}