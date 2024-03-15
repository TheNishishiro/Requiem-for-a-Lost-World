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
		private float _actualOffset;
		
		public override void Attack()
		{
			var offset = isDualStrike ? 0.5f : 0;
			var spawnCount = isDualStrike ? 2 : 1;
			
			for (var i = 0; i < spawnCount; i++)
			{
				_actualOffset = offset * (i % 2 == 0 ? 1 : -1);
				base.Attack();
			}
		}

		public override void SetupProjectile(NetworkProjectile networkProjectile)
		{
			var playerTransform = GameManager.instance.PlayerTransform;
			var playerPosition = transform.position;
			var position = new Vector3(playerPosition.x + _actualOffset, playerPosition.y, playerPosition.z) + playerTransform.transform.forward;
			position = Utilities.GetPointOnColliderSurface(position, playerTransform.transform);
			
			networkProjectile.Initialize(this, position);
			networkProjectile.GetProjectile<GroundSlashProjectile>().SetDirection(playerTransform.transform.forward);
		}

		protected override void OnLevelUp()
		{
			if (LevelField == 9) 
				isDualStrike = true;
		}
	}
}