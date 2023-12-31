﻿using System;
using DefaultNamespace;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
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
        
		protected override bool ProjectileSpawn(LaserGunProjectile projectile)
		{
			var position = transform.position;
			var worldPosition = new Vector3(
				Random.Range(position.x - 2, position.x + 2),
				Random.Range(position.y, position.y + 2),
				Random.Range(position.z - 2, position.z - 0.5f));

			projectile.transform.position = worldPosition;
			projectile.SetStats(weaponStats);
			if (GameData.IsCharacterWithRank(CharactersEnum.Amelia_BoD, CharacterRank.E2))
				_healthComponent.Damage(-1f);
			return true;
		}

		protected override int GetAttackCount()
		{
			var count = weaponStats.GetAttackCount();
			
			return count;
		}
	}
}