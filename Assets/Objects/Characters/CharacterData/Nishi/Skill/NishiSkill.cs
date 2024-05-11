using System;
using Data.Elements;
using DefaultNamespace;
using Objects.Enemies;
using Objects.Players.Scripts;
using Objects.Stage;
using UnityEngine;
using Weapons;

namespace Objects.Characters.Nishi.Skill
{
	public class NishiSkill : CharacterSkillBase
	{
		private float _damage;
		private ElementalWeapon _elementalWeapon;
		
		private void Start()
		{
			_damage = GameData.GetPlayerCharacterRank() >= CharacterRank.E4 ? 300 : 50;
			_elementalWeapon = new ElementalWeapon(Element.Lightning);
			CurrentTimeToLive = LifeTime;
			TimeAlive = 0;

			StopAllStages();
			State = ProjectileState.Unspecified;
			SetState(ProjectileState.Spawning);
		}

		protected override void TickLifeTime()
		{
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!other.CompareTag("Enemy")) return;
            
			other.GetComponent<Damageable>().TakeDamage(PlayerStatsScaler.GetScaler().GetScaledDamageDealt(_damage).Damage, _elementalWeapon);
		}
	}
}