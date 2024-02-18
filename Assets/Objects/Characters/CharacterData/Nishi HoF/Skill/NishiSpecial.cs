using System;
using Data.Elements;
using DefaultNamespace;
using Events.Handlers;
using Events.Scripts;
using Managers;
using Objects.Abilities;
using Objects.Players.Scripts;
using Objects.Stage;
using UnityEngine;
using UnityEngine.Pool;
using Weapons;
using Random = UnityEngine.Random;

namespace Objects.Characters.Nishi_HoF.Skill
{
    public class NishiSpecial : CharacterSkillBase, IDamageTakenHandler, ISpecialBarFilledHandler, IDamageDealtHandler
    {
        [SerializeField] private GameObject lancePrefab;
        private ObjectPool<LanceProjectile> _lancePool;
        private Vector3 _targetPosition;
        private WeaponBase _lanceWeapon;
        
        private SpecialBarManager _specialBarManager;
        private SpecialBarManager SpecialBarManager
        {
            get
            {
                if (_specialBarManager == null)
                    _specialBarManager = FindAnyObjectByType<SpecialBarManager>();
                return _specialBarManager;
            }
        }

        private void Start()
        {
            GameManager.instance.playerComponent.SetCharacterState(PlayerCharacterState.Nishi_HoF_Flame_State);
            _lanceWeapon = new ElementalWeapon(Element.Cosmic);
            _lanceWeapon.weaponStats = new WeaponStats()
            {
                Damage = 15,
                Scale = 1,
                TimeToLive = 1
            };
            _lanceWeapon.WeaponStatsStrategy = new WeaponStatsStrategyBase(_lanceWeapon.weaponStats, Element.Cosmic);
            
            _lancePool = new ObjectPool<LanceProjectile>(
                () =>
                {
                    var lance = SpawnManager.instance.SpawnObject(transform.position, lancePrefab).GetComponent<LanceProjectile>();
                    lance.Init(_lancePool, lance);
                    lance.SetParentWeapon(_lanceWeapon);
                    return lance;
                }, 
                lance =>
                {
                    lance.SetParentWeapon(_lanceWeapon);
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
            DamageTakenEvent.Register(this);
            SpecialBarFilledEvent.Register(this);
            DamageDealtEvent.Register(this);
        }
    
        private void OnDisable()
        {
            DamageTakenEvent.Unregister(this);
            SpecialBarFilledEvent.Unregister(this);
            DamageDealtEvent.Unregister(this);
        }
        
        public void OnDamageTaken(float damage)
        {
            if (GameManager.IsCharacterState(PlayerCharacterState.Nishi_HoF_Flame_State))
                SpecialBarManager.Increment(5);
        }

        public void OnDamageDealt(Damageable damageable, float damage, bool isRecursion)
        {
            if (isRecursion || !GameData.IsCharacterRank(CharacterRank.E2)) return;

            if (!GameManager.IsCharacterState(PlayerCharacterState.Nishi_HoF_Void_State)) return;
            SpecialBarManager.Increment(0.1f);
            if (Random.value >= 0.5f) return;
                
            _targetPosition = damageable.transform.position + new Vector3(0,2,0);
            _lancePool.Get();
        }

        public void OnSpecialBarFilled()
        {
            var newState = GameManager.IsCharacterState(PlayerCharacterState.Nishi_HoF_Flame_State)
                    ? PlayerCharacterState.Nishi_HoF_Void_State
                    : PlayerCharacterState.Nishi_HoF_Flame_State;
            
            GameManager.instance.playerComponent.SetCharacterState(newState);
            if (newState == PlayerCharacterState.Nishi_HoF_Flame_State)
                GameManager.instance.playerComponent.playerVfxComponent.PlayFlameTransform();
            else
                GameManager.instance.playerComponent.playerVfxComponent.PlayVoidTransform();
            
            SpecialBarManager.ResetBar();
        }
    }
}