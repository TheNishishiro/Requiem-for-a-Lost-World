using System;
using Data.Elements;
using Interfaces;
using Objects.Characters;
using Objects.Enemies;
using Objects.Stage;
using UnityEngine;
using Weapons;
using Random = UnityEngine.Random;

namespace Objects.Abilities.Corruption_Aura
{
    public class CorruptedAuraProjectile : StagableProjectile
    {
        private Transform _targetTransform;
        
        public void FollowTransform(Transform target)
        {
            _targetTransform = target;
        }

        protected override void CustomUpdate()
        {
            transformCache.position = _targetTransform.position;
            transformCache.rotation = Quaternion.Euler(0,0,0);
            var result = Vector3.one * WeaponStatsStrategy.GetScale();
            transformCache.localScale = new Vector3(result.x, 1, result.z);
        }

        public void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                var enemy = other.GetComponent<Enemy>();
                var damageCooldown = WeaponStatsStrategy.GetDamageCooldown();
                if (GameData.IsCharacterWithRank(CharactersEnum.Lucy_BoC, CharacterRank.E3))
                    damageCooldown -= 0.2f;

                enemy.GetDamagableComponent().TakeDamageWithCooldown(WeaponStatsStrategy.GetDamageDealt(), gameObject, damageCooldown, ParentWeapon);
                if (GameData.IsCharacterWithRank(CharactersEnum.Lucy_BoC, CharacterRank.E2))
                    enemy.GetChaseComponent().SetSlow(1f, 0.3f);
                if (GameData.IsCharacterWithRank(CharactersEnum.Lucy_BoC, CharacterRank.E4) && Time.frameCount % 30 == 0)
                {
                    var values = Enum.GetValues(typeof(Element));
                    var resistanceShredElement = (Element)values.GetValue(Random.Range(0, values.Length));
                    enemy.GetDamagableComponent().ReduceElementalDefence(resistanceShredElement, 0.1f);
                }
            }
            else if (other.CompareTag("Destructible"))
            {
                var damageable = other.GetComponent<IDamageable>();
                damageable.TakeDamageWithCooldown(WeaponStatsStrategy.GetDamageDealt(), gameObject, WeaponStatsStrategy.GetDamageCooldown(), ParentWeapon);
            }
        }
    }
}