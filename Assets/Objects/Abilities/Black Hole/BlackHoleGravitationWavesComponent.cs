using Objects.Enemies;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Back_Hole
{
	public class BlackHoleGravitationWavesComponent : ProjectileBase
	{
		[SerializeField] private GameObject blackHoleCenter;
		private float enemiesCount;

		public override void SetParentWeapon(WeaponBase parentWeapon)
		{
			base.SetParentWeapon(parentWeapon);
			enemiesCount = 10;
		}

		private void OnTriggerStay(Collider other)
		{
			if (enemiesCount-- <= 0)
				return;
			
			var enemyComponent = other.GetComponent<Enemy>();
			if (enemyComponent != null)
			{
				enemyComponent.GetChaseComponent().SetTemporaryTarget(blackHoleCenter, 5f, TimeLeftToLive);
				enemyComponent.SetNoCollisions(TimeLeftToLive);
			}
		}
	}
}