using System;
using System.Collections;
using Cinemachine;
using Data.Elements;
using DefaultNamespace;
using Objects.Stage;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Weapons;

namespace Objects.Characters.Nishi_HoF.Skill
{
    public class NishiSkill : CharacterSkillBase
    {
        private float _damage;
        private ElementalWeapon _elementalWeapon;
        [SerializeField] private float damageDelay;
		
        private void Start()
        {
            var baseDamage = GameData.GetPlayerCharacterRank() >= CharacterRank.E4 ? 300 : 50;
            _damage = baseDamage * (1 + GameData.GetPlayerCharacterData().Stats.DamagePercentageIncrease);
            _elementalWeapon = new ElementalWeapon(Element.Fire);
            CurrentTimeToLive = LifeTime;
            TimeAlive = 0;

            StopAllStages();
            State = ProjectileState.Unspecified;
            SetState(ProjectileState.Spawning);

            StartCoroutine(DamageCountdown());
            StartCoroutine(ExplosionFlash());
        }

        private IEnumerator DamageCountdown()
        {
            yield return new WaitForSeconds(damageDelay);
            EnemyManager.instance.GlobalDamage(_damage, _elementalWeapon);
        }
        
        private IEnumerator ExplosionFlash()
        {
            yield return new WaitForSeconds(2.9f);
            var volume = FindAnyObjectByType<Volume>();
            volume.profile.TryGet(out ShadowsMidtonesHighlights _smh);
            volume.profile.TryGet(out ColorAdjustments _ca);
            volume.profile.TryGet(out ChromaticAberration _chromaticAberration);

            var cinemachineVirtualCamera = FindAnyObjectByType<CinemachineVirtualCamera>();
            var perlinNoise = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            perlinNoise.m_AmplitudeGain = 2;

            _smh.active = true;
            _ca.active = true;
            _chromaticAberration.active = true;

            yield return new WaitForSeconds(0.4f);
			
            _smh.active = false;
            _ca.active = false;
            _chromaticAberration.active = false;

            yield return new WaitForSeconds(1f);
            
            perlinNoise.m_AmplitudeGain = 0;
        }
    }
}