using Data.Elements;
using DefaultNamespace;
using Objects.Stage;
using UnityEngine;

namespace Objects.Characters.Adam.Skill
{
	public class AdamSkill : CharacterSkillBase
	{
		private float _damage;
		private ElementalWeapon _elementalWeapon;
		
		private void Start()
		{
			var baseDamage = GameData.GetPlayerCharacterRank() >= CharacterRank.E4 ? 150 : 50;
			_damage = baseDamage * (1 + GameData.GetPlayerCharacterData().Stats.DamagePercentageIncrease);
			_elementalWeapon = new ElementalWeapon(Element.Cosmic);
		}

		private void Update()
		{
			TickLifeTime();
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!other.CompareTag("Enemy")) return;
            
			other.GetComponent<Damageable>().TakeDamage(_damage, _elementalWeapon);
		}
	}
}