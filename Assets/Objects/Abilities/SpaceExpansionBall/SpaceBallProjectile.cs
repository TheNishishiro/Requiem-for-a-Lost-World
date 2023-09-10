using System.Collections;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.SpaceExpansionBall
{
	public class SpaceBallProjectile : ProjectileBase
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

		public void SetDirection(float dirX, float dirY, float dirZ)
		{
			direction = (new Vector3(dirX, dirY, dirZ) - transform.position).normalized;
			direction.y = 0;
		}

		void Update()
		{
			TickLifeTime();
			
			if (state == State.Traveling)
				transform.position += direction * (WeaponStats.GetSpeed() * Time.deltaTime);
			
			if ((TimeToLive <= WeaponStats.GetTimeToLive() / 2 || enemiesHit > WeaponStats.GetPassThroughCount()) && state == State.Traveling)
			{
				state = State.Exploding;
				transform.localScale *= WeaponStats.GetScale();
				ProjectileDamageIncreasePercentage = 0.1f;
				StartCoroutine(Enlarge());
			}
		}
		
		protected override void OnLifeTimeEnd()
		{
			_isDead = true;
			if (!SpaceBallWeapon.IsGallacticCollapse)
				Destroy(gameObject);
			
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
			var explosion = Instantiate(SpaceBallWeapon.ExplosionPrefab, transform.position, Quaternion.identity);
			var projectileComponent = explosion.GetComponent<SimpleDamageProjectile>();
			
			projectileComponent.SetParentWeapon(ParentWeapon);
			projectileComponent.SetStats(new WeaponStats()
			{
				TimeToLive = 0.5f,
				Damage = WeaponStats.GetDamage() * 2, 
				Scale = WeaponStats.GetScale(),
			});
			
			
			yield return new WaitForSeconds(1f);
			Destroy(gameObject);
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