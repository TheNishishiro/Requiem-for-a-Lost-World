using System;
using System.Collections;
using Data.Elements;
using DefaultNamespace;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Fire_Ring
{
    public class FireRingProjectile : PoolableProjectile<FireRingProjectile>
    {
        public float maxSize;
        public float growthRate;
        public float expansionTime;
        private float _currentExpansionTime;
        private Vector3 _baseSize;
        private FireRingWeapon FireRingWeapon => (FireRingWeapon)ParentWeapon;

        public override void SetStats(WeaponStats weaponStats)
        {
            base.SetStats(weaponStats);
            _currentExpansionTime = 0;
            _baseSize = transformCache.localScale;
        }

        protected override void CustomUpdate()
        {
            var actualGrowthRate = growthRate * (FireRingWeapon.IsInfernoTrail ? 2 : 1);
            _currentExpansionTime += actualGrowthRate * Time.deltaTime;

            var interpolationStep = _currentExpansionTime / expansionTime;
            transformCache.localScale = Vector3.Lerp(_baseSize, _baseSize * maxSize, interpolationStep);
        }

        private void OnTriggerEnter(Collider other)
        {
            DamageOverTime(other);
            if (FireRingWeapon.IsSearingHeat)
                other.GetComponent<Damageable>()?.ReduceElementalDefence(Element.Fire, 0.35f);
        }
    }
}