using System;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Wind_Shear
{
    public class WindShearProjectile : PoolableProjectile<WindShearProjectile>
    {
        public WindSheerWeapon WindShearWeapon => (WindSheerWeapon)ParentWeapon;
        [SerializeField] private MeshRenderer windOuterRenderer;
        [SerializeField] private float windOuterMaxDissolve;
        [SerializeField] private MeshRenderer windInnerRenderer;
        [SerializeField] private float windInnerMaxDissolve;
        [SerializeField] private ParticleSystem particles;
        [SerializeField] private float dissolveTime;
        private static readonly int Dissolve = Shader.PropertyToID("_Dissolve");

        private void OnTriggerEnter(Collider other)
        {
            DamageOverTime(other);
            if (WindShearWeapon.IsHurricaneWrath)
            {
                var chaseComponent = other.GetComponent<ChaseComponent>();
                if (chaseComponent != null)
                    chaseComponent.SetSlow(0.3f, 1);
            }
        }
        
        protected override void CustomUpdate()
        {
            if (TimeAlive < dissolveTime)
            {
                SetDissolveValues(true);
            }
            else if (CurrentTimeToLive < dissolveTime)
            {
                if (particles.isEmitting)
                    particles.Stop(true);
                SetDissolveValues(false);
            }
        }

        private void SetDissolveValues(bool isAnticipation)
        {
            SetRendererDissolve(windOuterRenderer, windOuterMaxDissolve, GetDissolveValue(windOuterMaxDissolve, isAnticipation));
            SetRendererDissolve(windInnerRenderer, windInnerMaxDissolve, GetDissolveValue(windInnerMaxDissolve, isAnticipation));
        }

        private float GetDissolveValue(float max, bool isAnticipation)
        {
            return isAnticipation ? Mathf.Lerp(0f, max, TimeAlive/dissolveTime) : Mathf.Lerp(0f, max, CurrentTimeToLive/dissolveTime);
        }

        private void SetRendererDissolve(MeshRenderer renderer, float maxDissolve, float dissolveValue)
        {
            var material = renderer.materials[0];
            var clampedDissolve = Mathf.Clamp(dissolveValue, 0f, maxDissolve);
            material.SetFloat(Dissolve, clampedDissolve);
        }
    }
}