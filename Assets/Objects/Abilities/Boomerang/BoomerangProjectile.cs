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
			StartCoroutine(FlyCoroutine());
		}
		
		private void SetDirectionInternal(Vector3 targetPoint)
		{
			direction = (targetPoint - transform.position).normalized;
		}

		private IEnumerator FlyCoroutine()
		{
			yield return new WaitForSeconds(WeaponStats.GetTimeToLive()*0.25f);
			SetDirectionInternal(ParentWeapon.transform.position);
		}
		
		void Update()
		{
			TickLifeTime();
			transform.Rotate(0, Time.deltaTime * 250, 0);
			transform.position += direction * (WeaponStats.GetSpeed() * Time.deltaTime);
		}

		private void OnTriggerEnter(Collider other)
		{
			SimpleDamage(other, false);
		}
	}
}