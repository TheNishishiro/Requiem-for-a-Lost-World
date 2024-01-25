using UnityEngine;
using Weapons;

namespace Objects.Abilities.Meteor
{
	public class MeteorProjectile : PoolableProjectile<MeteorProjectile>
	{
		private Vector3 direction;
		[SerializeField] private ParticleSystem explosionParticleSystem;
		
		public void SetDirection(float dirX, float dirY, float dirZ)
		{
			direction = (new Vector3(dirX, dirY, dirZ) - transformCache.position).normalized;
		}

		protected override void CustomUpdate()
		{
			transformCache.position += direction * ((WeaponStatsStrategy?.GetSpeed()).GetValueOrDefault() * Time.deltaTime);
			var ray = new Ray(transformCache.position, Vector3.down);
			var layer = LayerMask.GetMask("FloorLayer");
			var raycastResult = Physics.Raycast(ray, out var hit, Mathf.Infinity, layer);
				
			if (raycastResult && hit.distance < 0.5f)
			{
				SetState(ProjectileState.Dissipating);
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			SimpleDamage(other, false);
		}
	}
}