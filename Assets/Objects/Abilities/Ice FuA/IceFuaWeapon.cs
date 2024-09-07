using System;
using System.Collections;
using DefaultNamespace.Data.Statuses;
using DefaultNamespace.Data.Weapons;
using Events.Handlers;
using Events.Scripts;
using Managers;
using Objects.Enemies;
using Unity.Netcode;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Ice_FuA
{
    public class IceFuaWeapon : PoolableWeapon<IceFuaSpawner>, IExpPickedUpHandler
    {
        private int _expShardPickedUp;
        private Vector3 _startPosition;
        private Enemy _target;
        private int _shardsRequired = 50;

        private void OnEnable()
        {
            ExpPickedUpEvent.Register(this);
        }

        private void OnDisable()
        {
            ExpPickedUpEvent.Unregister(this);
        }

        public void OnExpPickedUp(float amount)
        {
            _expShardPickedUp++;
            if (_expShardPickedUp > _shardsRequired)
            {
                Attack();
                _expShardPickedUp = 0;
            }
            
            StatusEffectManager.instance.AddOrRemoveEffect(StatusEffectType.IceFuaWeaponStacks, _expShardPickedUp);
        } 
        
        public override void Attack()
        {
            RpcManager.instance.FireProjectileRpc(WeaponId, transform.position, NetworkManager.Singleton.LocalClientId, WeaponPoolEnum.Main);
        }

        public void SpawnSubProjectile(Vector3 startPos, Enemy target)
        {
            _startPosition = startPos;
            _target = target;
            RpcManager.instance.FireProjectileRpc(WeaponId, transform.position, NetworkManager.Singleton.LocalClientId, WeaponPoolEnum.Sub);
        }
        
        public override void SetupProjectile(NetworkProjectile networkProjectile, WeaponPoolEnum weaponPoolId)
        {
            switch (weaponPoolId)
            {
                case WeaponPoolEnum.Main:
                    networkProjectile.Initialize(this, transform.position);
                    break;
                case WeaponPoolEnum.Sub:
                    networkProjectile.Initialize(this, _startPosition);
                    networkProjectile.GetProjectile<IceFuaProjectile>().SetTarget(_target);
                    break;
            }
        }

        protected override void OnLevelUp()
        {
            if (LevelField == 5)
                _shardsRequired = 40;
        }

        protected override IEnumerator AttackProcess()
        {
            yield break;
        }
    }
}