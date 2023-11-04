using System;
using System.Linq;
using DefaultNamespace;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Managers;
using Objects.Characters;
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
			projectile.SetStats(weaponStats);
			projectile.SetDirection(position.x, position.y, position.z);
			return true;
		}

		protected override int GetAttackCount()
		{
			var baseCount = base.GetAttackCount();
			if (GameData.GetPlayerCharacterId() == CharactersEnum.David_BoF)
			{
				baseCount += 2;
				if (GameData.GetPlayerCharacterRank() >= CharacterRank.E5)
					baseCount += (int)Math.Ceiling(baseCount * 0.5f);
			}

			return baseCount;
		}
	}
}