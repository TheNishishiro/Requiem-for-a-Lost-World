using System;
using DefaultNamespace;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Managers;
using Objects.Characters;
using Objects.Stage;
using UnityEditor.Experimental;
using UnityEngine;
using Weapons;
using Random = UnityEngine.Random;

namespace Objects.Abilities.Laser_Gun
{
	public class LaserGunWeapon : PoolableWeapon<LaserGunProjectile>
	{
		protected override bool ProjectileSpawn(LaserGunProjectile projectile)
		{
			var position = transform.position;
			var worldPosition = new Vector3(
				Random.Range(position.x - 2, position.x + 2),
				Random.Range(position.y, position.y + 2),
				Random.Range(position.z - 2, position.z - 0.5f));

			projectile.transform.position = worldPosition;
			projectile.SetStats(weaponStats);
			return true;
		}

		protected override int GetAttackCount()
		{
			var count = weaponStats.GetAttackCount();
			if (GameData.GetPlayerCharacterId() == CharactersEnum.Amelia_BoD &&
			    GameData.GetPlayerCharacterRank() >= CharacterRank.E4)
				count += 2;

			return count;
		}
	}
}