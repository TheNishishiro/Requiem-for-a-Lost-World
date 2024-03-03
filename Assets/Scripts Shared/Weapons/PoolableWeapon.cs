using DefaultNamespace;
using Managers;
using NaughtyAttributes;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Pool;

namespace Weapons
{
    public abstract class PoolableWeapon<T> : WeaponBase where T : PoolableProjectile<T>
    {
        [Foldout("Pooling")]
        [SerializeField] public int capacity = 50;
        [Foldout("Pooling")]
        [SerializeField] public int maxCapacity = 50;
        protected ObjectPool<T> pool;
        
        public override void Attack()
        {
            if (useNetworkPool)
            {
                RpcManager.instance.FireProjectileRpc(WeaponId, transform.position, NetworkManager.Singleton.LocalClientId);

            }
            else
            {
                if (pool.CountActive >= maxCapacity)
                    return;

                pool.Get();
            }
        }
        
        protected override void InitPool()
        {
            pool = new ObjectPool<T>(ProjectileInitInternal, ProjectileSpawnInternal, projectile => projectile.gameObject.SetActive(false), Destroy, false, capacity, maxCapacity);
        }
        
        private T ProjectileInitInternal()
        {
            var entity = ProjectileInit();
            entity.Init(pool, entity);
            return entity;
        }

        protected virtual T ProjectileInit()
        {
            var projectile = SpawnManager.instance.SpawnObject(transform.position, spawnPrefab).GetComponent<T>();
            projectile.SetParentWeapon(this);
            projectile.Init(pool, projectile);
            return projectile;
        }

        private void ProjectileSpawnInternal(T projectile)
        {
            projectile.gameObject.SetActive(ProjectileSpawn(projectile));
        }
        
        protected abstract bool ProjectileSpawn(T projectile);
    }
}