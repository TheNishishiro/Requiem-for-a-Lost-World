using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Ground_Slash
{
	public class GroundSlashProjectile : PoolableProjectile<GroundSlashProjectile>
	{
		private Vector3 direction;
		[SerializeField] private Rigidbody rigidbody;
		
		public void SetDirection(Vector3 dir)
		{
			direction = dir;
			transformCache.rotation = Quaternion.LookRotation(direction.normalized) * Quaternion.Euler(0,0,0);
			StartCoroutine(SlowDown());
		}

		private void OnTriggerEnter(Collider other)
		{
			SimpleDamage(other, false);
		}

		private IEnumerator SlowDown()
		{
			var startSpeed = WeaponStatsStrategy.GetSpeed();
			var elapsedLifetime = 0f;
			var startScale = transformCache.localScale;
			var endScale = startScale*0.2f;

			while (!IsDead && elapsedLifetime < GetTimeToLive())
			{
				var speed = Mathf.Lerp(startSpeed, 0, elapsedLifetime / GetTimeToLive());
				rigidbody.linearVelocity = direction.normalized * speed;
				transformCache.localScale = Vector3.Lerp(startScale, endScale, elapsedLifetime / GetTimeToLive());
				elapsedLifetime += Time.deltaTime;
				yield return null;
			}
		}
	}
}