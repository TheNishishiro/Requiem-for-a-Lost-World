using System;
using System.Collections;
using DefaultNamespace;
using Events.Handlers;
using Events.Scripts;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Low_Pressure
{
    public class LowPressureWeapon : PoolableWeapon<LowPressureProjectile>, IDamageDealtHandler
    {
        private Vector3 _targetPosition;
        private bool _canAttack;
        
        public override void SetupProjectile(NetworkProjectile networkProjectile)
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
            _canAttack = true;
        }

        public void OnDamageDealt(Damageable damageable, float damage, bool isRecursion, WeaponEnum weaponId)
        {
            if (!_canAttack || isRecursion)
                return;
            
            _targetPosition = damageable.GetTargetPosition();
            Attack();
            _canAttack = false;
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