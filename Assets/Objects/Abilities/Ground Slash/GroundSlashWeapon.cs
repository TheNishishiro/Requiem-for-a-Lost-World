using System.Linq;
using DefaultNamespace;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;
using Weapons;

namespace Objects.Abilities.Ground_Slash
{
	public class GroundSlashWeapon : WeaponBase
	{
		public bool isDualStrike = true;
		public bool isShatteredEarth;
		
		public override void Attack()
		{
			var playerTransform = FindObjectsOfType<Player>().FirstOrDefault();

			var offset = isDualStrike ? 0.5f : 0;
			var playerPosition = transform.position;
			var spawnCount = isDualStrike ? 2 : 1;
			
			for (var i = 0; i < spawnCount; i++)
			{
				var actualOffset = offset * (i % 2 == 0 ? 1 : -1);
				var position = new Vector3(playerPosition.x + actualOffset, playerPosition.y, playerPosition.z);
				var groundSlash = SpawnManager.instance.SpawnObject(Utilities.GetPointOnColliderSurface(position, playerTransform.transform), spawnPrefab);
				var projectileComponent = groundSlash.GetComponent<GroundSlashProjectile>();
			
				projectileComponent.SetParentWeapon(this);
				projectileComponent.SetStats(weaponStats);
				projectileComponent.SetDirection(playerTransform.transform.forward);
			}
		}

		protected override void OnLevelUp()
		{
			switch (LevelField)
			{
				case 7:
					isDualStrike = true;
					break;
				case 9:
					isShatteredEarth = true;
					break;
			}
		}
	}
}