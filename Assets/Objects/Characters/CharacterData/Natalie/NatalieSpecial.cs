using System;
using Data.Elements;
using DefaultNamespace;
using Events.Handlers;
using Events.Scripts;
using Objects.Players.Scripts;
using Objects.Stage;
using UnityEngine;

namespace Objects.Characters.Natalie
{
	public class NatalieSpecial : CharacterSkillBase, IDamageOverTimeExpiredHandler
	{
		private PlayerStatsComponent _playerStatsComponent;
		private ElementalWeapon _elementalWeapon;

		private void Start()
		{
			_playerStatsComponent = GetComponentInParent<PlayerStatsComponent>();
			_elementalWeapon = new ElementalWeapon(Element.Wind);
		}

		public void OnEnable()
		{
			DamageOverTimeExpiredHandler.Register(this);
		}
		
		public void OnDisable()
		{
			DamageOverTimeExpiredHandler.Unregister(this);
		}

		public void OnDoTExpired(Damageable damageable, float damage)
		{
			if (damageable != null)
				damageable.TakeDamage(damage * 4f, _elementalWeapon);
		}
	}
}