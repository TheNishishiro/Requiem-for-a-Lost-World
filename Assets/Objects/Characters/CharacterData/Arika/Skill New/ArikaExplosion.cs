using System;
using Data.Elements;
using DefaultNamespace;
using Objects.Stage;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using ChromaticAberration = UnityEngine.Rendering.Universal.ChromaticAberration;
using Random = UnityEngine.Random;

namespace Objects.Characters.Chronastra.Skill
{
    public class ArikaExplosion : CharacterSkillBase
    {
        private ElementalWeapon _elementalWeapon;
        private Volume _volume;
        private float _deBuffDuration;
        private float _deBuffModifier;
        private void Start()
        {
            _elementalWeapon = new ElementalWeapon(Element.Cosmic);
            _volume =  FindFirstObjectByType<Volume>();
        }

        public void SetDuration(float duration)
        {
            ParticleSystem.Stop();
            var main = ParticleSystem.main;
            main.duration = LifeTime = TimeToLive = duration;
            _deBuffDuration = GameData.IsCharacterRank(CharacterRank.E5) ? 4 : 1;
            _deBuffModifier = GameData.IsCharacterRank(CharacterRank.E5) ? 1 : 0.5f;
            ParticleSystem.Play();
        }

        private void Update()
        {
            TickLifeTime();
        }
        
        protected override void OnDestroy()
        {
            _volume.profile.TryGet(out ChromaticAberration ca);
            ca.active = false;
            
            base.OnDestroy();
        }

        private void OnTriggerStay(Collider other)
        {
            if (!other.CompareTag("Enemy")) return;
            var damageableComponent = other.GetComponent<Damageable>();
            if (GameData.IsCharacterRank(CharacterRank.E2))
                damageableComponent.TakeDamageWithCooldown(2, gameObject, 1f, _elementalWeapon);
            damageableComponent.SetTakeAdditionalDamageFromAllSources(_elementalWeapon, _deBuffDuration, _deBuffModifier);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            
            _volume.profile.TryGet(out ChromaticAberration ca);
            ca.active = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            
            _volume.profile.TryGet(out ChromaticAberration ca);
            ca.active = false;
        }
    }
}