using DefaultNamespace;
using Managers;
using Weapons;

namespace Objects.Abilities.Boomerang
{
	public class BoomerangWeapon : PoolableWeapon<BoomerangProjectile>
	{
		protected override bool ProjectileSpawn(BoomerangProjectile projectile)
		{
			var transform1 = transform;
			var position = transform1.position;
			
			projectile.transform.position = position;
			var targetPoint = Utilities.GetRandomInAreaFreezeParameter(position, 3, isFreezeY: true);
			projectile.SetStats(weaponStats);
			projectile.gameObject.SetActive(true);
			projectile.SetDirection(targetPoint);
			return true;
		}
	}
}