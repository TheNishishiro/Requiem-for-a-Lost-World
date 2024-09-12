using UnityEngine;
using Weapons;

public class LightningOrbProjectile : PoolableProjectile<LightningOrbProjectile>
{
    private void OnTriggerStay(Collider other)
    {
        DamageArea(other, out _);
    }
}
