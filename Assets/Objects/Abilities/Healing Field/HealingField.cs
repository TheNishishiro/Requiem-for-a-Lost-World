using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Objects.Abilities.Healing_Field
{
	public class HealingField : MonoBehaviour
	{
		[SerializeField] private float lifeTime;
		private float _healAmount;

		private void Update()
		{
			lifeTime -= Time.deltaTime;
			if (lifeTime <= 0)
			{
				Destroy(gameObject);
			}
		}

		public void SetHealAmount(float healAmount)
		{
			_healAmount = healAmount;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Player"))
			{
				other.GetComponent<Player>().TakeDamage(-_healAmount);
			}
		}
	}
}