using System;
using Data.Elements;
using Interfaces;
using UnityEngine;
using UnityEngine.Serialization;
using Weapons;

namespace Objects.Abilities.Wind_Shear
{
    public class SandStormProjectile : PoolableProjectile<SandStormProjectile>
    {
        public SandStormWeapon SandStormWeapon => (SandStormWeapon)ParentWeapon;
        [SerializeField] private MeshRenderer SandInnerRenderer;
        [SerializeField] private float sandInnerMaxDissolve;
        [SerializeField] private float sandOuterMaxDissolve;
        [SerializeField] private float dissolveTime;
        [SerializeField] private Renderer particleRenderer;
        private static readonly int Dissolve = Shader.PropertyToID("_AlphaFadeAmount");

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Enemy")) return;
            var damageable = other.GetComponent<IDamageable>();
            DamageOverTime(damageable, other);
            if (SandStormWeapon.IsSuffocatingGrit)
                damageable.ReduceElementalDefence(Element.Earth, 0.35f);
        }
        
        protected override void CustomUpdate()
        {
            if (TimeAlive < dissolveTime)
            {
                SetDissolveValues(true);
            }
            else if (CurrentTimeToLive < dissolveTime)
            {
                SetDissolveValues(false);
            }
        }

        private void SetDissolveValues(bool isAnticipation)
        {
            SetRendererDissolve(particleRenderer, GetDissolveValue(sandOuterMaxDissolve, isAnticipation));
            SetRendererDissolve(SandInnerRenderer, GetDissolveValue(sandInnerMaxDissolve, isAnticipation));
        }

        private float GetDissolveValue(float max, bool isAnticipation)
        {
            return isAnticipation ? Mathf.Lerp(1f, max, TimeAlive/dissolveTime) : Mathf.Lerp(1f, max, CurrentTimeToLive/dissolveTime);
        }

        private void SetRendererDissolve(Renderer renderer, float dissolveValue)
        {
            var material = renderer.materials[0];
            material.SetFloat(Dissolve, dissolveValue);
        }
    }
}