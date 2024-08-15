using System;
using DefaultNamespace;
using DefaultNamespace.Data.Weapons;
using Managers;
using NaughtyAttributes;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Pool;

namespace Weapons
{
    public abstract class PoolableWeapon<T> : WeaponBase where T : PoolableProjectile<T>
    {
        public override void Attack()
        {
            RpcManager.instance.FireProjectileRpc(WeaponId, transform.position, NetworkManager.Singleton.LocalClientId, WeaponPoolEnum.Main);
        }
    }
}