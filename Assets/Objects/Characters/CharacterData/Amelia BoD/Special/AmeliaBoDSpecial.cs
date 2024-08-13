using System;
using Data.Elements;
using DefaultNamespace;
using Events.Handlers;
using Events.Scripts;
using Managers;
using Objects.Abilities;
using Objects.Players.Scripts;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;
using Weapons;

namespace Objects.Characters.Amelia_BoD.Special
{
    public class AmeliaBoDSpecial : CharacterSkillBase, IDamageDealtHandler, ISpecialBarFilledHandler
    {
        [SerializeField] private GameObject followUpPrefab;
        [SerializeField] private float InternalProjectileCooldown = 0.2f;
        private ObjectPool<AmeliaBoDFollowUpProjectile> _projectilePool;
        private Vector3 _targetPosition;
        private WeaponBase _followUpWeapon;
        private float _specialStateTimer;
        private float _internalCooldown;

        private void Start()
        {
            _followUpWeapon = new ElementalWeapon(Element.Light);
            _followUpWeapon.weaponStats = new WeaponStats()
            {
                Damage = 25,
                Scale = 1,
                TimeToLive = 1
            };
            _followUpWeapon.WeaponStatsStrategy = new WeaponStatsStrategyBase(_followUpWeapon.weaponStats, Element.Cosmic);
            _projectilePool = new ObjectPool<AmeliaBoDFollowUpProjectile>(
                () =>
                {
                    var followUp = SpawnManager.instance.SpawnObject(transform.position, followUpPrefab).GetComponent<AmeliaBoDFollowUpProjectile>();
                    followUp.Init(_projectilePool, followUp);
                    followUp.SetParentWeapon(_followUpWeapon);
                    return followUp;
                }, 
                lance =>
                {
                    lance.SetParentWeapon(_followUpWeapon);
                    lance.transform.position = _targetPosition;
                    lance.gameObject.SetActive(true);
                }, 
                lance =>
                {
                    lance.gameObject.SetActive(false);
                }, 
                lance =>
                {
                    Destroy(lance.gameObject);
                }, true, 300);
        }

        private void OnEnable()
        {
            DamageDealtEvent.Register(this);
            SpecialBarFilledEvent.Register(this);
        }

        private void OnDisable()
        {
            DamageDealtEvent.Unregister(this);
            SpecialBarFilledEvent.Unregister(this);
        }
        
        protected override void Update()
        {
            if (_internalCooldown > 0f)
                _internalCooldown -= Time.deltaTime;

            switch (_specialStateTimer)
            {
                case > 0f:
                    _specialStateTimer -= Time.deltaTime;
                    break;
                case <= 0 when GameManager.IsCharacterState(PlayerCharacterState.Amelia_BoD_EnchancedState):
                    GameManager.instance.playerComponent.SetCharacterState(PlayerCharacterState.None);
                    break;
            }
        }

        public void OnSpecialBarFilled()
        {
            GameManager.instance.playerComponent.SetCharacterState(PlayerCharacterState.Amelia_BoD_EnchancedState);
            _specialStateTimer = 20f;
            SpecialBarManager.instance.ResetBar();
        }

        public void OnDamageDealt(Damageable damageable, float damage, bool isRecursion, WeaponEnum weaponId)
        {
            var weapon = WeaponManager.instance.GetUnlockedWeapon(weaponId);
            if (weapon.ElementField != Element.Light)
                return;
            
            if (!GameManager.IsCharacterState(PlayerCharacterState.Amelia_BoD_EnchancedState))
                SpecialBarManager.instance.Increment();
            else if (_internalCooldown <= 0)
            {
                _targetPosition = damageable.GetTargetPosition();
                _projectilePool.Get();
                _internalCooldown = InternalProjectileCooldown;
            }
        }
    }
}