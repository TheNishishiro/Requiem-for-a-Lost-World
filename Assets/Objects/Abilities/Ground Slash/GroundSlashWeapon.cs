using System.Linq;
using DefaultNamespace;
using Managers;
using Weapons;

namespace Objects.Abilities.Ground_Slash
{
	public class GroundSlashWeapon : WeaponBase
	{
		public override void Attack()
		{
			var playerTransform = FindObjectsOfType<Player>().FirstOrDefault();
			
			var groundSlash = SpawnManager.instance.SpawnObject(Utilities.GetPointOnColliderSurface(transform.position, playerTransform.transform), spawnPrefab);
			var projectileComponent = groundSlash.GetComponent<GroundSlashProjectile>();
			
			projectileComponent.SetParentWeapon(this);
			projectileComponent.SetStats(weaponStats);
			projectileComponent.SetDirection(playerTransform.transform.forward);
		}
	}
}