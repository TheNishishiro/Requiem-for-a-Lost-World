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
		private Rigidbody _rigidbody;
		
		public void SetDirection(Vector3 dir)
		{
			_rigidbody = GetComponent<Rigidbody>();
			direction = dir;
			transform.rotation = Quaternion.LookRotation(direction.normalized) * Quaternion.Euler(0,0,0);
			StartCoroutine(SlowDown());
		}

		private void OnCollisionEnter(Collision other)
		{
			SimpleDamage(other.collider, false);
		}

		private IEnumerator SlowDown()
		{
			var startSpeed = WeaponStatsStrategy.GetSpeed();
			var elapsedLifetime = 0f;
			var startScale = transform.localScale;
			var endScale = startScale*0.2f;

			while (!IsDead && elapsedLifetime < GetTimeToLive())
			{
				var speed = Mathf.Lerp(startSpeed, 0, elapsedLifetime / GetTimeToLive());
				_rigidbody.velocity = direction.normalized * speed;
				transform.localScale = Vector3.Lerp(startScale, endScale, elapsedLifetime / GetTimeToLive());
				elapsedLifetime += Time.deltaTime;
				yield return null;
			}
		}
	}
}