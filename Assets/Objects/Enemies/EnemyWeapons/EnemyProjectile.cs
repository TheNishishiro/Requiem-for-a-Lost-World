using DefaultNamespace;
using Managers;
using Unity.Netcode;
using UnityEngine;
using Weapons;

namespace Objects.Enemies.EnemyWeapons
{
    public class EnemyProjectile<T> : PoolableProjectile<T> where T : StagableProjectile
    {
        protected override void Destroy()
        {
            var prefab = ParentWeapon.GetProjectile();
            NetworkObjectPool.Singleton.ReturnNetworkObject(networkProjectile.networkObject, prefab);
            networkProjectile.networkObject.Despawn(false);
        }

        protected void SimpleDamage(Collider collisionInfo)
        {
            if (collisionInfo.gameObject.CompareTag("Player"))
            {
                var networkObject = collisionInfo.gameObject.GetComponent<NetworkObject>();
                var damage = WeaponStatsStrategy.GetDamageDealt(ProjectileDamageIncreasePercentage, ProjectileDamageIncrease);
                if (networkObject.IsOwner)
                    GameManager.instance.playerComponent.TakeDamage(damage.Damage);
                else
                    RpcManager.instance.AttackPlayerRpc(networkObject.OwnerClientId, damage.Damage);
                
                OnLifeTimeEnd();
            }
        }
    }
}