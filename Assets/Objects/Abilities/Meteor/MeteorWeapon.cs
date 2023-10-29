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
			var spawnPosition = new Vector3(transform.position.x, transform.position.y + 5.0f, transform.position.z);
			var enemy = FindObjectsByType<Damageable>(FindObjectsSortMode.None).OrderBy(x => Random.value).FirstOrDefault();
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

		public override bool IsUnlocked(SaveFile saveFile)
		{
			return saveFile.IsAchievementUnlocked(AchievementEnum.Survive15MinutesWithDavid);
		}
	}
}