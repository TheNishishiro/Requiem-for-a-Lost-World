using DefaultNamespace;
using Managers;
using Weapons;

namespace Objects.Abilities.Katana
{
	public class KatanaWeapon : PoolableWeapon<KatanaProjectile>
	{
		protected override bool ProjectileSpawn(KatanaProjectile projectile)
		{
			var slashPosition = transform.position + transform.forward;
			projectile.transform.position = slashPosition;
			projectile.SetStats(weaponStats);

			return true;
		}
	}
}