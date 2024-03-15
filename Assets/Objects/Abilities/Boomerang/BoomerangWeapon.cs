using DefaultNamespace;
using Managers;
using Weapons;

namespace Objects.Abilities.Boomerang
{
	public class BoomerangWeapon : PoolableWeapon<BoomerangProjectile>
	{
		public override void SetupProjectile(NetworkProjectile networkProjectile)
		{
			var transform1 = transform;
			var position = transform1.position;
			
			var targetPoint = Utilities.GetRandomInAreaFreezeParameter(position, 3, isFreezeY: true);
			networkProjectile.Initialize(this, position);
			networkProjectile.GetProjectile<BoomerangProjectile>().SetDirection(targetPoint);
		}
	}
}