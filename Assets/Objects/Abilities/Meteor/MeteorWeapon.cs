using System;
using System.Linq;
using DefaultNamespace;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Managers;
using Objects.Characters;
using Objects.Players.Scripts;
using Objects.Stage;
using UnityEngine;
using Weapons;
using Random = UnityEngine.Random;

namespace Objects.Abilities.Meteor
{
	public class MeteorWeapon : PoolableWeapon<MeteorProjectile>
	{
		protected override bool ProjectileSpawn(MeteorProjectile projectile)
		{
			var position1 = transform.position;
			var spawnPosition = new Vector3(position1.x, position1.y + 5.0f, position1.z);
			var enemy = EnemyManager.instance.GetRandomEnemy();
			if (enemy == null)
				return false;
			
			var position = enemy.transform.position;
			projectile.transform.position = spawnPosition;
			projectile.SetParentWeapon(this);
			projectile.SetDirection(position.x, position.y, position.z);

			if (GameData.IsCharacterWithRank(CharactersEnum.David_BoF, CharacterRank.E2))
			{
				var hpReductionCeiling = GameData.IsCharacterRank(CharacterRank.E5) ? 0.4f : 0.6f;
				var hpDiff = PlayerStatsScaler.GetScaler().GetHealth() / PlayerStatsScaler.GetScaler().GetMaxHealth();

				if (hpDiff > hpReductionCeiling)
				{
					GameManager.instance.playerComponent.TakeDamage(2, true, true);
				}
			}
			
			return true;
		}

		protected override int GetAttackCount()
		{
			var baseCount = base.GetAttackCount();
			if (GameData.GetPlayerCharacterId() == CharactersEnum.David_BoF)
			{
				baseCount += 2;
				if (GameData.GetPlayerCharacterRank() >= CharacterRank.E1)
					baseCount += (int)Math.Ceiling(baseCount * 0.5f);
			}

			return baseCount;
		}
	}
}