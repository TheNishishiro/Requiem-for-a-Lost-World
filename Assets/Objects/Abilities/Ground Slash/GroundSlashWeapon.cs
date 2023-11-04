using System.Linq;
using DefaultNamespace;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;
using Weapons;

namespace Objects.Abilities.Ground_Slash
{
	public class GroundSlashWeapon : PoolableWeapon<GroundSlashProjectile>
	{
		public bool isDualStrike = true;
		public bool isShatteredEarth;
		private float _actualOffset;
		
		public override void Attack()
		{
			var offset = isDualStrike ? 0.5f : 0;
			var spawnCount = isDualStrike ? 2 : 1;
			
			for (var i = 0; i < spawnCount; i++)
			{
				_actualOffset = offset * (i % 2 == 0 ? 1 : -1);
				pool.Get();
			}
		}

		protected override bool ProjectileSpawn(GroundSlashProjectile projectile)
		{
			var playerTransform = GameManager.instance.playerComponent.transform;
			var playerPosition = transform.position;
			var position = new Vector3(playerPosition.x + _actualOffset, playerPosition.y, playerPosition.z);
			projectile.transform.position = Utilities.GetPointOnColliderSurface(position, playerTransform.transform);
			
			projectile.gameObject.SetActive(true);
			projectile.SetParentWeapon(this);
			projectile.SetStats(weaponStats);
			projectile.SetDirection(playerTransform.transform.forward);
			return true;
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