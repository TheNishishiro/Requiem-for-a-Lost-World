using System;
using Objects.Players.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Objects.Abilities.Healing_Field
{
	public class HealingField : MonoBehaviour
	{
		[SerializeField] private float lifeTime;
		private float _healAmount;
		private bool _isEmpowering;

		private void Update()
		{
			lifeTime -= Time.deltaTime;
			if (lifeTime <= 0)
			{
				Destroy(gameObject);
			}
		}

		public void Setup(float healAmount, bool isEmpowering)
		{
			_healAmount = healAmount;
			_isEmpowering = isEmpowering;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!other.CompareTag("Player")) return;
			var playerComponent = other.GetComponent<Player>();
			playerComponent.TakeDamage(-_healAmount);
				
			if (_isEmpowering)
				other.GetComponent<PlayerStatsComponent>().TemporaryAttackBoost(0.5f, 1.5f);
		}
	}
}