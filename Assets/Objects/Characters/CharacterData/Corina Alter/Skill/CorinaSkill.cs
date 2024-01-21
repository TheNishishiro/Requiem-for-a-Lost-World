using System;
using System.Collections;
using Data.Elements;
using DefaultNamespace;
using Objects.Players.Scripts;
using UnityEngine;

namespace Objects.Characters.Corina_Alter.Skill
{
	public class CorinaSkill : CharacterSkillBase
	{
		private PlayerStatsComponent _playerStatsComponent;
		private HealthComponent _healthComponent;
		private BoxCollider _collider;
		private ElementalWeapon _elementalWeapon;

		private void Start()
		{
			_playerStatsComponent = FindFirstObjectByType<PlayerStatsComponent>();
			_healthComponent = FindFirstObjectByType<HealthComponent>();
			_elementalWeapon = new ElementalWeapon(Element.Physical);
			_collider = GetComponent<BoxCollider>();
			StartCoroutine(ColliderKeepAlive());
			StartCoroutine(RiseAnimation());
		}
		
		private void Update()
		{
			TickLifeTime();
		}

		private IEnumerator ColliderKeepAlive()
		{
			yield return new WaitForSeconds(0.3f);
			_collider.enabled = false;
		}

		private IEnumerator RiseAnimation()
		{
			var duration = 0.3f;
			var elapsed = 0.0f;
			var randomYRotation = UnityEngine.Random.Range(0f, 360f);
			
			var startRotation = Quaternion.Euler(0, randomYRotation, -120);
			var endRotation = Quaternion.Euler(0, randomYRotation, 20); 

			while (elapsed < duration)
			{
				var time = Mathf.Clamp(elapsed / duration, 0.0f, 1.0f);
				transform.rotation = Quaternion.Lerp(startRotation, endRotation, time);

				elapsed += Time.deltaTime;
				yield return null;
			}

			transform.rotation = endRotation;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Enemy"))
			{
				other.GetComponent<Damageable>().TakeDamage(25 + PlayerStatsScaler.GetScaler().GetDamage(), _elementalWeapon);
				_healthComponent.Damage(-3);
			}
		}
	}
}