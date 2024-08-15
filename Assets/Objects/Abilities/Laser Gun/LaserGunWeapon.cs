using System;
using DefaultNamespace;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using DefaultNamespace.Data.Weapons;
using Managers;
using Objects.Characters;
using Objects.Players.Scripts;
using Objects.Stage;
using UnityEditor.Experimental;
using UnityEngine;
using Weapons;
using Random = UnityEngine.Random;

namespace Objects.Abilities.Laser_Gun
{
	public class LaserGunWeapon : PoolableWeapon<LaserGunProjectile>
	{
		private HealthComponent _healthComponent;
		public override void Awake()
		{
			_healthComponent = FindFirstObjectByType<HealthComponent>();
			base.Awake();
			if (GameData.IsCharacterWithRank(CharactersEnum.Amelia_BoD, CharacterRank.E1))
			{
				weaponStats.DamageCooldown -= 0.15f;
				weaponStats.TimeToLive *= 1.5f;
			}

			if (GameData.IsCharacterWithRank(CharactersEnum.Amelia_BoD, CharacterRank.E3))
			{
				weaponStats.DetectionRange += 1f;
				weaponStats.AttackCount += 1;
			}
		}

		public override void SetupProjectile(NetworkProjectile networkProjectile, WeaponPoolEnum weaponPoolId)
		{
			var position = transform.position;
			var worldPosition = new Vector3(
				Random.Range(position.x - 2, position.x + 2),
				Random.Range(position.y, position.y + 2),
				Random.Range(position.z - 2, position.z - 0.5f));

			networkProjectile.Initialize(this, worldPosition);
			if (GameData.IsCharacterWithRank(CharactersEnum.Amelia_BoD, CharacterRank.E2))
				_healthComponent.Damage(-1f);
		}

		protected override int GetAttackCount()
		{
			var count = WeaponStatsStrategy.GetAttackCount();
			
			return count;
		}
	}
}