using DefaultNamespace.Data.Weapons;
using Interfaces;
using Managers;
using Objects;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Pool;

namespace Weapons
{
    public class PoolableProjectile<T> : StagableProjectile where T : MonoBehaviour
    {
        protected ObjectPool<T> _objectPool;
        protected T _object;
        
        public void Init(ObjectPool<T> objectPool, T entity)
        {
            _objectPool = objectPool;
            _object = entity;
        }

        protected override void Destroy()
        {
            if (ParentWeapon.IsUseNetworkPool())
                RpcManager.instance.DespawnProjectileRpc(networkProjectile.networkObject, (WeaponEnum)ParentWeapon.GetId(), projectileTypeId);
            else
                ReturnToPool(_objectPool, _object);
        }
    }
}