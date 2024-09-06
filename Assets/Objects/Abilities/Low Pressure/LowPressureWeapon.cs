using System;
using System.Collections;
using DefaultNamespace;
using DefaultNamespace.Data.Weapons;
using Events.Handlers;
using Events.Scripts;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Low_Pressure
{
    public class LowPressureWeapon : PoolableWeapon<LowPressureProjectile>, IDamageDealtHandler
    {
        private Vector3 _targetPosition;
        private float _innerCooldown;
        
        public override void SetupProjectile(NetworkProjectile networkProjectile, WeaponPoolEnum weaponPoolId)
        {
            if (_targetPosition == Vector3.zero)
                return;
            networkProjectile.Initialize(this, _targetPosition);
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