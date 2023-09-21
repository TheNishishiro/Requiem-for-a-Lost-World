using System;
using Data.Elements;
using DefaultNamespace;
using Objects.Players.Scripts;
using UnityEngine;

namespace Objects.Characters.Arika.Skill
{
	public class ArikaStar : MonoBehaviour
	{
		private PlayerStatsComponent _playerStatsComponent;
		private BoxCollider _collider;
		private float colliderTime = 0.8f;
		private ElementalWeapon _elementalWeapon;
		
		private void Start()
		{
			_playerStatsComponent = FindObjectOfType<PlayerStatsComponent>();
			_collider = GetComponent<BoxCollider>();
			_elementalWeapon = new ElementalWeapon(Element.Cosmic);
		}
		
		private void Update()
		{
			colliderTime -= Time.deltaTime;
			if (colliderTime <= 0)
			{
				_collider.enabled = false;
			}
		}
		
		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Enemy"))
			{
				other.GetComponent<Damageable>().TakeDamage(_playerStatsComponent.GetTotalDamage(15), _elementalWeapon);
			}
		}
	}
}