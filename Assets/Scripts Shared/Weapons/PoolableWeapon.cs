using DefaultNamespace;
using Managers;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Pool;

namespace Weapons
{
    public abstract class PoolableWeapon<T> : WeaponBase where T : PoolableProjectile<T>
    {
        [Foldout("Pooling")]
        [SerializeField] public int capacity = 50;
        [Foldout("Pooling")]
        [SerializeField] public int maxCapacity = 200;
        protected ObjectPool<T> pool;
        public override void Attack()
        {
            pool.Get();
        }
        
        protected override void InitPool()
        {
            pool = new ObjectPool<T>(ProjectileInit, ProjectileSpawnInternal, projectile => projectile.gameObject.SetActive(false), Destroy, false, capacity, maxCapacity);
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
            ProjectileSpawn(projectile);
            projectile.gameObject.SetActive(true);
        }
        
        protected abstract void ProjectileSpawn(T projectile);
    }
}