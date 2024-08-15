using System;
using System.Collections;
using DefaultNamespace;
using DefaultNamespace.Data.Weapons;
using Events.Handlers;
using Events.Scripts;
using Managers;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Pool;
using Weapons;
using Random = UnityEngine.Random;

namespace Objects.Abilities.Portal
{
    public class PortalWeapon : PoolableWeapon<PortalProjectile>, IDamageDealtHandler
    {
        private Vector3 _targetPosition;
        private Vector3 _spawnPosition;
        private float _innerCooldown;

        public override void Attack()
        {
            RpcManager.instance.FireProjectileRpc(WeaponId, transform.position, NetworkManager.Singleton.LocalClientId, WeaponPoolEnum.Main);
            RpcManager.instance.FireProjectileRpc(WeaponId, transform.position, NetworkManager.Singleton.LocalClientId, WeaponPoolEnum.Sub);
        }

        public override void SetupProjectile(NetworkProjectile networkProjectile, WeaponPoolEnum weaponPoolId)
        {
            if (_targetPosition == Vector3.zero)
                return;
            
            networkProjectile.Initialize(this, _spawnPosition);
            switch (weaponPoolId)
            {
                case WeaponPoolEnum.Main:
                    networkProjectile.GetProjectile<PortalProjectile>().SetTarget(_targetPosition);
                    break;
                case WeaponPoolEnum.Sub:
                    networkProjectile.GetProjectile<PortalSubProjectile>().SetDirection(_targetPosition.x, _targetPosition.y, _targetPosition.z);
                    break;
            }
        }

        protected override IEnumerator AttackProcess()
        {
            yield break;
        }

        protected override void CustomUpdate()
        {
            if (_innerCooldown > 0)
                _innerCooldown -= Time.deltaTime;
        }

        public void OnDamageDealt(Damageable damageable, float damage, bool isRecursion, WeaponEnum weaponId)
        {
            if (_innerCooldown > 0 || isRecursion)
                return;
            
            _targetPosition = damageable.GetTargetPosition();
            _spawnPosition =  new Vector3(
                Random.Range(_targetPosition.x - 2, _targetPosition.x + 2),
                Random.Range(_targetPosition.y, _targetPosition.y + 2),
                Random.Range(_targetPosition.z - 2, _targetPosition.z - 0.5f));
            _innerCooldown = WeaponStatsStrategy.GetDuplicateSpawnDelay();
            Attack();
        }
        
        private void OnEnable()
        {
            DamageDealtEvent.Register(this);
        }

        private void OnDisable()
        {
            DamageDealtEvent.Unregister(this);
        }
    }
}