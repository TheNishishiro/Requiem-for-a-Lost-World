using Objects.Enemies;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Back_Hole
{
	public class BlackHoleGravitationWavesComponent : ProjectileBase
	{
		[SerializeField] private NetworkProjectile networkProjectile;
		[SerializeField] private GameObject blackHoleCenter;

		private void OnTriggerStay(Collider other)
		{
			if (!networkProjectile.IsHost) return;
			
			var enemyComponent = other.GetComponent<Enemy>();
			if (enemyComponent != null)
			{
				enemyComponent.GetChaseComponent().SetTemporaryTarget(blackHoleCenter, 5f, 0.5f);
				enemyComponent.SetNoCollisions(0.5f);
			}
		}
	}
}