using Objects.Enemies;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Back_Hole
{
	public class BlackHoleGravitationWavesComponent : ProjectileBase
	{
		[SerializeField] private GameObject blackHoleCenter;

		public override void SetParentWeapon(WeaponBase parentWeapon)
		{
			base.SetParentWeapon(parentWeapon);
		}

		private void OnTriggerStay(Collider other)
		{
			var enemyComponent = other.GetComponent<Enemy>();
			if (enemyComponent != null)
			{
				enemyComponent.GetChaseComponent().SetTemporaryTarget(blackHoleCenter, 5f, TimeLeftToLive);
				enemyComponent.SetNoCollisions(TimeLeftToLive);
			}
		}
	}
}