using System;
using DefaultNamespace;
using Objects.Enemies;
using Objects.Stage;
using UnityEngine;

namespace Objects.Characters.Nishi.Skill
{
	public class NishiSkill : CharacterSkillBase
	{
		private float _damage;
		
		private void Start()
		{
			var baseDamage = GameData.GetPlayerCharacterRank() >= CharacterRank.E4 ? 150 : 50;
			_damage = baseDamage * (1 + GameData.GetPlayerCharacterData().Stats.DamagePercentageIncrease);
		}

		private void Update()
		{
			TickLifeTime();
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!other.CompareTag("Enemy")) return;
            
			other.GetComponent<Damageable>().TakeDamage(_damage);
		}
	}
}