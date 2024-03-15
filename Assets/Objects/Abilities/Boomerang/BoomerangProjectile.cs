using System.Collections;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Boomerang
{
	public class BoomerangProjectile : PoolableProjectile<BoomerangProjectile>
	{
		private Vector3 direction;

		public void SetDirection(Vector3 targetPoint)
		{
			SetDirectionInternal(targetPoint);
		}

		protected override void OnStateChanged(ProjectileState state)
		{
			if (state == ProjectileState.Flying)
				StartCoroutine(FlyCoroutine());
		}

		private void SetDirectionInternal(Vector3 targetPoint)
		{
			direction = (targetPoint - transformCache.position).normalized;
		}

		private IEnumerator FlyCoroutine()
		{
			yield return new WaitForSeconds(WeaponStatsStrategy.GetTotalTimeToLive()*0.25f);
			SetDirectionInternal(ParentWeapon.transform.position);
		}
		
		protected override void CustomUpdate()
		{
			transformCache.Rotate(0, Time.deltaTime * 250, 0);
			transformCache.position += direction * (WeaponStatsStrategy.GetSpeed() * Time.deltaTime);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (ProjectileDamageIncreasePercentage > -0.5f)
				ProjectileDamageIncreasePercentage -= 0.02f;
				
			SimpleDamage(other, false);
		}
	}
}