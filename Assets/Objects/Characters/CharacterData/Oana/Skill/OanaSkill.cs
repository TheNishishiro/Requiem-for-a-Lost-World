using System;
using Data.Elements;
using DefaultNamespace;
using Objects.Stage;
using UnityEngine;

namespace Objects.Characters.Oana.Skill
{
    public class OanaSkill : CharacterSkillBase
    {
        private float _damage;
        private ElementalWeapon _elementalWeapon;
        
        private void Start()
        {
            _damage = 10 * (1 + GameData.GetPlayerCharacterData().Stats.DamagePercentageIncrease);
            _elementalWeapon = new ElementalWeapon(Element.Ice);
        }
        
        private void Update()
        {
            TickLifeTime();
        }

        private void OnTriggerStay(Collider other)
        {
            if (!other.CompareTag("Enemy")) return;

            var damageable = other.GetComponent<Damageable>();
            damageable.TakeDamageWithCooldown(_damage, gameObject, 0.5f, _elementalWeapon);
            if (!GameData.IsCharacterRank(CharacterRank.E2)) return;
            
            var chaseComponent = other.GetComponentInParent<ChaseComponent>();
            if (chaseComponent != null)
                chaseComponent.SetSlow(1.0f, 0.8f);
            
            if (GameData.IsCharacterRank(CharacterRank.E3))
                damageable.ReduceElementalDefence(Element.Ice, 0.05f);
        }
    }
}