using System.Collections;
using Interfaces;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.SpaceExpansionBall
{
	public class SpaceBallProjectile : PoolableProjectile<SpaceBallProjectile>
	{
		private int enemiesHit = 0;
		private Vector3 direction;
		private enum State
		{
			Traveling,
			Exploding,
			Developed
		}
		private State state = State.Traveling;
		private SpaceBallWeapon SpaceBallWeapon => ParentWeapon as SpaceBallWeapon;

		public override void SetStats(IWeaponStatsStrategy weaponStatsStrategy)
		{
			base.SetStats(weaponStatsStrategy);
			state = State.Traveling;
			enemiesHit = 0;
		}

		public void SetDirection(float dirX, float dirY, float dirZ)
		{
			direction = (new Vector3(dirX, dirY, dirZ) - transform.position).normalized;
			direction.y = 0;
		}

		protected override void CustomUpdate()
		{
			if (state == State.Traveling)
				transform.position += direction * (WeaponStatsStrategy.GetSpeed() * Time.deltaTime);
			
			if ((CurrentTimeToLive <= WeaponStatsStrategy.GetTotalTimeToLive() / 2 || enemiesHit > WeaponStatsStrategy.GetPassThroughCount()) && state == State.Traveling)
			{
				state = State.Exploding;
				transform.localScale *= WeaponStatsStrategy.GetScale();
				ProjectileDamageIncreasePercentage = 0.7f;
				StartCoroutine(Enlarge());
			}
		}
		
		protected override void OnLifeTimeEnd()
		{
			IsDead = true;
			if (!SpaceBallWeapon.IsGallacticCollapse)
			{
				Destroy();
				return;
			}

			StartCoroutine(Collapse());
		}

		private IEnumerator Enlarge()
		{
			var increaseTimes = 0;
			while (state == State.Exploding)
			{
				if (increaseTimes >= 10)
					state = State.Developed;
				
				increaseTimes++;
				transform.localScale *= 1.1f;
				transform.localPosition.Scale(new Vector3(0,1.1f,0));
				yield return new WaitForSeconds(0.1f);
			}
			
			ProjectileDamageIncreasePercentage = -0.75f;
		}

		private IEnumerator Collapse()
		{
			// Collapse object on itself
			var scale = transform.localScale;
			while (scale.x > 0)
			{
				scale.x -= 0.2f;
				scale.y -= 0.2f;
				scale.z -= 0.2f;
				transform.localScale = scale;
				yield return new WaitForSeconds(0.01f);
			}
			
			// Spawn explosion
			SpaceBallWeapon.SpawnSubProjectile(transformCache.position);
			
			yield return new WaitForSeconds(1f);
			Destroy();
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Enemy"))
				enemiesHit++;
			
			if (state == State.Traveling)
				SimpleDamage(other, false);
			else
				DamageArea(other, out _);
			
		}
	}
}