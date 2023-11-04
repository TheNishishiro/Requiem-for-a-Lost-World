using DefaultNamespace;
using Managers;
using Weapons;

namespace Objects.Abilities.Katana
{
	public class KatanaWeapon : PoolableWeapon<KatanaProjectile>
	{
		protected override bool ProjectileSpawn(KatanaProjectile projectile)
		{
			var transform1 = transform;
			
			var slashPosition = transform1.position + transform1.forward/2;
			projectile.transform.position = slashPosition;
			projectile.SetStats(weaponStats);

			return true;
		}
	}
}