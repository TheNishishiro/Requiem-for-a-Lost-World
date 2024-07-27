using System;
using Data.Elements;
using DefaultNamespace;
using Events.Handlers;
using Events.Scripts;
using Managers;
using Objects.Abilities;
using Objects.Characters.Nishi_HoF.Skill;
using Objects.Characters.Yami_no_Tokiya.Special;
using Objects.Enemies;
using Objects.Players.Scripts;
using UnityEngine;
using UnityEngine.Pool;
using Weapons;

namespace Objects.Characters.Special
{
    public class YamiSpecial : CharacterSkillBase, IReactionTriggeredEvent
    {
        [SerializeField] private float flowerLifeTime;
        [SerializeField] private float flowerBaseDamage;
        [SerializeField] private float flowerSpawnCooldown;
        private float _currentSpawnCooldown = 0;
        private ObjectPool<AbyssFlowerProjectile> _objectPool;
        [SerializeField] private GameObject abyssFlowerPrefab; 
        private WeaponBase _followUpWeapon;
        private Vector3 _targetPosition;
        
        private void Start()
        {
            _followUpWeapon = new ElementalWeapon(Element.Cosmic);
            _followUpWeapon.weaponStats = new WeaponStats()
            {
                Damage = flowerBaseDamage,
                Scale = 1,
                TimeToLive = flowerLifeTime,
                DamageCooldown = 0.2f
            };
            _followUpWeapon.WeaponStatsStrategy = new AbyssFlowerStrategy(_followUpWeapon);
            _objectPool = new ObjectPool<AbyssFlowerProjectile>(
                () =>
                {
                    var abyssFlower = SpawnManager.instance.SpawnObject(_targetPosition, abyssFlowerPrefab).GetComponent<AbyssFlowerProjectile>();
                    abyssFlower.Init(_objectPool, abyssFlower);
                    return abyssFlower;
                }, 
                o =>
                {
                    o.transform.position = _targetPosition;
                    o.SetParentWeapon(_followUpWeapon);
                    o.gameObject.SetActive(true);
                }, 
                o => o.gameObject.SetActive(false), 
                o => Destroy(o.gameObject), 
                true, 75);
        }

        protected override void Update()
        {
            if (_currentSpawnCooldown > 0f)
                _currentSpawnCooldown -= Time.deltaTime;
        }

        private void OnEnable()
        {
            ReactionTriggeredEvent.Register(this);
        }

        private void OnDisable()
        {
            ReactionTriggeredEvent.Unregister(this);
        }

        public void OnReactionTriggered(ElementalReaction reaction, Damageable damageable)
        {
            if (_currentSpawnCooldown > 0f) return;
            _currentSpawnCooldown = flowerSpawnCooldown;
            _targetPosition = damageable.GetTargetPosition();
            _objectPool.Get();
        }

        public void SpawnFlowers(int count)
        {
            var t = transform;
            for (var i = 0; i < count; i++)
            {
                _targetPosition = Utilities.GetPointOnColliderSurface(Utilities.GetRandomInArea(t.position, 6f), t, 0.2f);
                _objectPool.Get();
            }
        }
    }
}