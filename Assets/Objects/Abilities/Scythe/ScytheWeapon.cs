using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Managers;
using Objects.Characters;
using Objects.Players.Scripts;
using Objects.Stage;
using Weapons;

namespace Objects.Abilities.Scythe
{
	public class ScytheWeapon : PoolableWeapon<ScytheProjectile>
	{
		private HealthComponent _healthComponent;
		public bool IsBloodEmbrace;
		public bool IsCursedStrikes;
		public bool IsSoulHarvest;

		public override void Awake()
		{
			_healthComponent = FindFirstObjectByType<HealthComponent>();
			base.Awake();
			if (GameData.IsCharacterWithRank(CharactersEnum.Corina_BoB, CharacterRank.E4))
				weaponStats.Scale += 0.5f;
		}

		protected override ScytheProjectile ProjectileInit()
		{
			var katanaSlash = Instantiate(spawnPrefab, transform).GetComponent<ScytheProjectile>();
			katanaSlash.SetParentWeapon(this);
			katanaSlash.SetPlayerHealthComponent(_healthComponent);
			return katanaSlash;
		}

		protected override bool ProjectileSpawn(ScytheProjectile projectile)
		{
			projectile.transform.position = transform.position;
			projectile.SetParentWeapon(this);
			return true;
		}

		protected override int GetAttackCount()
		{
			return 1;
		}

		protected override void OnLevelUp()
		{
			switch (LevelField)
			{
				case 5:
					IsBloodEmbrace = true;
					break;
				case 7:
					IsCursedStrikes = true;
					break;
				case 10:
					IsSoulHarvest = true;
					break;
			}
		}
	}
}