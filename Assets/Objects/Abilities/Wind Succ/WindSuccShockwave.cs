using UnityEngine;
using Weapons;

namespace Objects.Abilities.Wind_Succ
{
    public class WindSuccShockwave : PoolableProjectile<WindSuccShockwave>
    {
        protected override void Awake()
        {
            base.Awake();
            ProjectileDamageIncreasePercentage = 0.5f;
        }

        private void OnTriggerEnter(Collider other)
        {
            SimpleDamage(other, false);
        }
    }
}