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
		public bool IsBloodEmbrace;
		public bool IsCursedStrikes;
		public bool IsSoulHarvest;

		public override void Awake()
		{
			base.Awake();
			if (GameData.IsCharacterWithRank(CharactersEnum.Corina_BoB, CharacterRank.E4))
				weaponStats.Scale += 0.5f;
		}

		public override void SetupProjectile(NetworkProjectile networkProjectile)
		{
			networkProjectile.Initialize(this, transform.position);
			networkProjectile.Parent(GameManager.instance.PlayerTransform);
			networkProjectile.GetProjectile<ScytheProjectile>().SetPlayerHealthComponent(GameManager.instance.playerComponent.healthComponent);
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