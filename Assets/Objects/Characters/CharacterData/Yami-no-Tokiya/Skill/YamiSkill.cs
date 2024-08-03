using Data.Elements;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Game;
using DefaultNamespace.Data.Settings;
using Managers;
using Objects.Characters.Special;
using Objects.Players.PermUpgrades;
using Objects.Players.Scripts;
using Objects.Stage;
using UnityEngine;

namespace Objects.Characters.Yami_no_Tokiya.Skill
{
    public class YamiSkill : CharacterSkillBase
    {
        [SerializeField] private Material skyboxMaterial;
        [SerializeField] private LayerMask cameraCullingMask;
        [SerializeField] private Animator animator;
        [SerializeField] private AudioClip bassDropAudio;
        [SerializeField] private AudioClip fingerSnapAudio;
        private ElementalWeapon _elementalWeapon;
        private int _cullingMask;
        private Material _originalSkybox;
        private const float Damage = 50f;

        protected override void Awake()
        {
            var camera = Camera.main;
            _originalSkybox = RenderSettings.skybox;
            _cullingMask = camera.cullingMask;
            camera.cullingMask = cameraCullingMask;
            RenderSettings.skybox = skyboxMaterial;
            _elementalWeapon = new ElementalWeapon(Element.Cosmic);

            PauseManager.instance.AddPauseState(GamePauseStates.PauseTimer);
            PauseManager.instance.AddPauseState(GamePauseStates.PauseEnemyMovement);
            PauseManager.instance.AddPauseState(GamePauseStates.PauseWeaponAttacks);
            PauseManager.instance.AddPauseState(GamePauseStates.PausePlayerMovement);
            
            AudioManager.instance.PlaySound(bassDropAudio);
        }
        
        private void Update()
        {
            TickLifeTime();
            
            if (Input.GetKeyDown(SaveFile.Instance.GetKeybinding(KeyAction.Ability)))
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    AudioManager.instance.PlaySound(fingerSnapAudio);
                    animator.SetTrigger("Attack1");
                }
                else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
                {
                    AudioManager.instance.PlaySound(fingerSnapAudio);
                    animator.SetTrigger("Attack2");
                }
                else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
                {
                    AudioManager.instance.PlaySound(fingerSnapAudio);
                    animator.SetTrigger("Attack3");
                }
                else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3"))
                {
                    AudioManager.instance.PlaySound(fingerSnapAudio);
                    animator.SetTrigger("Attack4");
                }
            }

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Finish"))
                OnDestroy();
        }

        public void DealDamage()
        {
            EnemyManager.instance.GlobalDamage(PlayerStatsScaler.GetScaler().GetScaledDamageDealt(Damage).Damage, _elementalWeapon);
        }

        protected override void OnDestroy()
        {
            Camera.main.cullingMask = _cullingMask;
            RenderSettings.skybox = _originalSkybox;
            PauseManager.instance.ClearFullPause();
            if (GameData.IsCharacterRank(CharacterRank.E5))
            {
                GameManager.instance.playerStatsComponent.TemporaryStatBoost(StatEnum.CritRate, 1f, 10f);
                GameManager.instance.playerStatsComponent.TemporaryStatBoost(StatEnum.DamagePercentageIncrease, 2f, 10f);
            }
            base.OnDestroy();
        }
    }
}