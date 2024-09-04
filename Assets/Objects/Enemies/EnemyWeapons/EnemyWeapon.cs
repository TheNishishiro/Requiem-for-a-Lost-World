using System.Collections;
using Data.Elements;
using DefaultNamespace.Data.Game;
using DefaultNamespace.Data.Weapons;
using Interfaces;
using Managers;
using Objects.Abilities;
using Unity.Netcode;
using UnityEngine;
using Weapons;

namespace Objects.Enemies.EnemyWeapons
{
    public class EnemyWeapon : IWeapon
    {
        public GameObject spawnPrefab;
        public EnemyWeaponId weaponId;
        public WeaponStats weaponStats;
        protected EnemyWeaponStrategy EnemyWeaponStrategy;
        protected Transform transformCache;
        private ChaseComponent _chaseComponent;
        private float _timer;
        private readonly int _id = Random.Range(int.MinValue, int.MaxValue);

        public EnemyWeapon(GameObject spawnPrefab, WeaponStats weaponStats, EnemyWeaponId weaponId)
        {
            this.spawnPrefab = spawnPrefab;
            this.weaponStats = weaponStats;
            this.weaponId = weaponId;
        }
        
        public void InitWeapon(Transform transform)
        {
            transformCache = transform;
            CreateWeaponStrategy();
            _timer = Random.Range(EnemyWeaponStrategy.GetTotalCooldown()*.25f, EnemyWeaponStrategy.GetTotalCooldown()*1.5f);
        }

        protected virtual void CreateWeaponStrategy()
        {
            EnemyWeaponStrategy = new EnemyWeaponStrategy(weaponStats);
        }
        
        /// <summary>
        /// Update weapon cooldowns
        /// </summary>
        /// <returns>True when weapon is ready to send a projectile</returns>
        public virtual bool WeaponUpdate()
        {
            _timer -= Time.deltaTime;
            if (_timer >= 0f || PauseManager.instance.IsPauseStateSet(GamePauseStates.PauseEnemyMovement)) return false;

            _timer = EnemyWeaponStrategy.GetTotalCooldown();
            CustomUpdate();
            return true;
        }
        
        public virtual IEnumerator AttackProcess()
        {
            OnAttackStart();
            for (var i = 0; i < EnemyWeaponStrategy.GetAttackCount(); i++)
            {
                Attack();
                yield return new WaitForSeconds(EnemyWeaponStrategy.GetDuplicateSpawnDelay());
            }
            OnAttackEnd();
        }
        
        protected virtual void Attack()
        {
            var projectile = NetworkObjectPool.Singleton.GetNetworkObject(spawnPrefab, transformCache.position, Quaternion.identity);
            var netObj = projectile.GetComponent<NetworkObject>();
            var networkProjectile = netObj.GetComponent<NetworkProjectile>();
            SetupProjectile(networkProjectile);
            netObj.Spawn();
        }
        protected virtual void OnAttackStart() {}
        protected virtual void OnAttackEnd() {}
        public virtual void CustomUpdate() {}
        public virtual void SetupProjectile(NetworkProjectile networkProjectile) {}

        public GameObject GetProjectile()
        {
            return spawnPrefab;
        }

        public IWeaponStatsStrategy GetWeaponStrategy()
        {
            return EnemyWeaponStrategy;
        }

        public Transform GetTransform()
        {
            return transformCache;
        }

        public bool IsUseNetworkPool()
        {
            return true;
        }

        public int GetId()
        {
            return (int)weaponId;
        }

        public Element GetElement()
        {
            return Element.Disabled;
        }

        public int GetInstanceID()
        {
            return _id;
        }

        public void SetChaseComponent(ChaseComponent chaseComponent)
        {
            _chaseComponent = chaseComponent;
        }

        public Vector3 GetTarget()
        {
            return _chaseComponent.GetDestination();
        }
    }
}