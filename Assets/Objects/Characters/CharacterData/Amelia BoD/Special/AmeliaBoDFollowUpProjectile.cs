using UnityEngine;
using Weapons;

namespace Objects.Characters.Amelia_BoD.Special
{
    public class AmeliaBoDFollowUpProjectile : PoolableProjectile<AmeliaBoDFollowUpProjectile>
    {
        private void OnTriggerEnter(Collider other)
        {
            SimpleFollowUpDamage(other);
        }
    }
}