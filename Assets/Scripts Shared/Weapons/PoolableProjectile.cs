using Interfaces;
using UnityEngine;
using UnityEngine.Pool;

namespace Weapons
{
    public class PoolableProjectile<T> : StagableProjectile where T : MonoBehaviour
    {
        private ObjectPool<T> _objectPool;
        private T _object;
        
        public void Init(ObjectPool<T> objectPool, T entity)
        {
            _objectPool = objectPool;
            _object = entity;
        }

        protected override void Destroy()
        {
            ReturnToPool(_objectPool, _object);
        }
    }
}