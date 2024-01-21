using Objects.Abilities.Ice_Wave;
using Objects.Characters;
using Objects.Stage;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Time_Blade
{
    public class TimeBladeProjectile : PoolableProjectile<TimeBladeProjectile>
    {
        private TimeBladeWeapon TimeBladeWeapon => (TimeBladeWeapon)ParentWeapon;
        
        private void OnTriggerEnter(Collider other)
        {
            SimpleDamage(other, false);
            var chaseComponent = other.GetComponentInParent<ChaseComponent>();
            if (chaseComponent == null) return;
            
            var immobileTime = TimeBladeWeapon.IsTemporalMastery ? 0.6f : 0.25f;
            if (GameData.IsCharacterWithRank(CharactersEnum.Truzi_BoT, CharacterRank.E1))
                immobileTime *= 2;
            
            chaseComponent.SetImmobile(immobileTime);
        }
    }
}