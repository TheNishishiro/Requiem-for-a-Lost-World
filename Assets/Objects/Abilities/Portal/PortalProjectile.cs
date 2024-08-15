using UnityEngine;
using Weapons;

namespace Objects.Abilities.Portal
{
    public class PortalProjectile : PoolableProjectile<PortalProjectile>
    {
        public void SetTarget(Vector3 targetPosition)
        {
            transformCache.LookAt(targetPosition);
        }
    }
}