using Objects.Enemies;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Back_Hole
{
	public class BlackHoleGravitationWavesComponent : ProjectileBase
	{
		[SerializeField] private GameObject blackHoleCenter;
		
		private void OnTriggerStay(Collider other)
		{
			var chaseComponent = other.GetComponentInParent<ChaseComponent>();
			if (chaseComponent != null)
				chaseComponent.SetTemporaryTarget(blackHoleCenter, 5f);

			other.GetComponentInParent<Enemy>()?.SetNoCollisions(1f);
		}
	}
}