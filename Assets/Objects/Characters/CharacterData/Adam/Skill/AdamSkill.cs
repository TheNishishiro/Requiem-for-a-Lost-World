using Data.Elements;
using DefaultNamespace;
using Objects.Players.Scripts;
using Objects.Stage;
using UnityEngine;

namespace Objects.Characters.Adam.Skill
{
	public class AdamSkill : CharacterSkillBase
	{
		private float _damage;
		private ElementalWeapon _elementalWeapon;
		private PlayerStatsComponent _playerStatsComponent;
		private PlayerStatsComponent playerStatsComponent
		{
			get
			{
				if (_playerStatsComponent == null)
				{
					_playerStatsComponent = FindObjectOfType<PlayerStatsComponent>();
				}

				return _playerStatsComponent;
			}
		}
		
		private void Start()
		{
			_damage = 50 * (1 + GameData.GetPlayerCharacterData().Stats.DamagePercentageIncrease);
			_elementalWeapon = new ElementalWeapon(Element.Cosmic);
			playerStatsComponent.IncreaseDamageTaken(0.01f);
			FindObjectOfType<HealthComponent>().Damage(-(playerStatsComponent.GetMaxHealth() * 0.8f));
		}

		private void Update()
		{
			TickLifeTime();
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!other.CompareTag("Enemy")) return;

			var damage = _damage;
			var characterRank = GameData.GetPlayerCharacterRank();
			
			if (characterRank >= CharacterRank.E1)
			{
				damage *= playerStatsComponent.GetCritRate() > Random.Range(0f, 1f) ? (float)playerStatsComponent.GetCritDamage() : 1.0f;
			}
			
			other.GetComponent<Damageable>().TakeDamage(damage, _elementalWeapon);

			if (characterRank >= CharacterRank.E2)
			{
				other.GetComponent<Damageable>().ReduceElementalDefence(Element.Cosmic, 0.5f);
			}
		}
	}
}