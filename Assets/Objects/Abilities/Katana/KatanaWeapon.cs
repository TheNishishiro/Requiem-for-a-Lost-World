using DefaultNamespace;
using Managers;
using Weapons;

namespace Objects.Abilities.Katana
{
	public class KatanaWeapon : WeaponBase
	{
		public override void Attack()
		{
			var slashPosition = transform.position + transform.forward;
			
			var katanaSlash = SpawnManager.instance.SpawnObject(slashPosition, spawnPrefab);
			katanaSlash.transform.rotation = transform.rotation;
			var projectileComponent = katanaSlash.GetComponent<KatanaProjectile>();

			projectileComponent.SetParentWeapon(this);
			projectileComponent.SetStats(weaponStats);
		}
	}
}