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
        private ObjectPool<AbyssFlowerProjectile> _objectPool;
        [SerializeField] private GameObject abyssFlowerPrefab; 
        private WeaponBase _followUpWeapon;
        private Vector3 _targetPosition;
        
        private void Start()
        {
            _followUpWeapon = new ElementalWeapon(Element.Cosmic);
            _followUpWeapon.weaponStats = new WeaponStats()
            {
                Damage = 15,
                Scale = 1,
                TimeToLive = 5f,
            };
            _followUpWeapon.WeaponStatsStrategy = new WeaponStatsStrategyBase(_followUpWeapon.weaponStats, Element.Cosmic);
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
            _targetPosition = damageable.GetTargetPosition();
            _objectPool.Get();
        }
    }
}