using Interfaces;
using Objects.Abilities;
using UnityEngine;
using UnityEngine.Pool;

namespace Weapons
{
    public class PoolableProjectileWithLimitedHitBox<T> : PoolableProjectile<T> where T : MonoBehaviour
    {
        [SerializeField] public Collider collider;
        [SerializeField] public float colliderLifeTime = 0.1f;
        [SerializeField] public float colliderStartLifeTime = 0f;
        
        protected void UpdateCollider()
        {
            if (TimeAlive > colliderLifeTime + colliderStartLifeTime)
                OnColliderEnd();
            else if (TimeAlive >= colliderStartLifeTime)
                OnColliderStart();
        }

        protected virtual void OnColliderStart()
        {
            if (!collider.enabled)
                collider.enabled = true;
        }

        protected virtual void OnColliderEnd()
        {
            if (collider.enabled)
                collider.enabled = false;
        }
    }
}