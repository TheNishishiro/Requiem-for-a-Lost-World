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
        private static readonly int Dissolve = Shader.PropertyToID("_AlphaFadeAmount");

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
            SetRendererDissolve(windOuterRenderer, GetDissolveValue(windOuterMaxDissolve, isAnticipation));
            SetRendererDissolve(windInnerRenderer, GetDissolveValue(windInnerMaxDissolve, isAnticipation));
        }

        private float GetDissolveValue(float max, bool isAnticipation)
        {
            return isAnticipation ? Mathf.Lerp(1f, max, TimeAlive/dissolveTime) : Mathf.Lerp(1f, max, CurrentTimeToLive/dissolveTime);
        }

        private void SetRendererDissolve(MeshRenderer renderer, float dissolveValue)
        {
            var material = renderer.materials[0];
            material.SetFloat(Dissolve, dissolveValue);
        }
    }
}