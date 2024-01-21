using System;
using Data.Elements;
using DefaultNamespace;
using Objects.Players.Scripts;
using Objects.Stage;
using UnityEngine;
using Weapons;

namespace Objects.Characters.Truzi.Skill
{
    public class TruziSkill : CharacterSkillBase
    {
        private WeaponBase _elementalWeapon;

        protected override void Awake()
        {
            base.Awake();
            _elementalWeapon = new ElementalWeapon(Element.Ice);
        }

        public void Update()
        {
            TickLifeTime();
        }

        public void OnTriggerEnter(Collider other)
        {
            if (GameData.IsCharacterRank(CharacterRank.E0)) return;
            
            if (!other.CompareTag("Enemy")) return;
            
            other.GetComponent<Damageable>().TakeDamage(PlayerStatsScaler.GetScaler().GetDamageDealt().Damage, _elementalWeapon);
        }
    }
}