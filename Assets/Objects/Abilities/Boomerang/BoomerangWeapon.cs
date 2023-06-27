using DefaultNamespace;
using Managers;
using Weapons;

namespace Objects.Abilities.Boomerang
{
	public class BoomerangWeapon : WeaponBase
	{
		public override void Attack()
		{
			var magicBall = SpawnManager.instance.SpawnObject(transform.position, spawnPrefab);
			var projectileComponent = magicBall.GetComponent<BoomerangProjectile>();
        
			var targetPoint = Utilities.GetRandomInAreaFreezeParameter(transform.position, 3, isFreezeY: true);
			projectileComponent.SetParentWeapon(this);
			projectileComponent.SetStats(weaponStats);
			projectileComponent.SetDirection(targetPoint);
		}
	}
}