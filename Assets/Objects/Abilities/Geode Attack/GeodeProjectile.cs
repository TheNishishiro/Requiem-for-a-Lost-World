using DefaultNamespace;
using Managers;
using Objects.Abilities.Ground_Slash;
using Objects.Players.PermUpgrades;
using Unity.Netcode;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Geode_Attack
{
    public class GeodeProjectile : PoolableProjectile<GeodeProjectile>
    {
        private GeodeWeapon GeodeWeapon => (GeodeWeapon)ParentWeapon;

        public override void SubProjectileTrigger(SimpleSubProjectile simpleSubProjectile, Collider other)
        {
            if (other.CompareTag("Player"))
                OnPlayerHit(other);
            if (other.CompareTag("Enemy"))
                OnEnemyHit(other);
        }

        public void OnEnemyHit(Collider other)
        {
            SimpleDamage(other, false);
        }

        public void OnPlayerHit(Collider other)
        {
            if (!GeodeWeapon.IsSeismicResonance)
                return;
            
            RpcManager.instance.TempStatIncrease(StatEnum.Armor, 0.2f, 9999f, "GeodeArmorBuff", other.GetComponent<NetworkObject>().OwnerClientId);
        }
    }
}