using System;
using Managers;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;
using Weapons;

namespace Objects.Abilities
{
    public class ProjectileEmitter : MonoBehaviour
    {
        [Header("Emitter Settings")]
        public float emitCount;
        public float lifeTime;
        public float emissionCooldown;
        
        [Header("Emitter Limits")]
        public bool loop;
        [ShowIf("ShowEmissionTime")]
        public float emissionTime;
        public bool useEmissionTriggerLimit;
        [ShowIf("useEmissionTriggerLimit")]
        public int emissionTriggerLimit;
        
        [Header("Emitter Pooling")]
        public int poolDefaultCapacity = 20;
        public int poolMaxCapacity = 40;
        
        [Header("Projectile Settings")]
        public bool changeSizeOverLifeTime;
        [ShowIf("changeSizeOverLifeTime")]
        [CurveRange(0, 0, 1, 1)]
        public AnimationCurve sizeOverLifeTime;
        public bool changeColorOverLifeTime;
        [ShowIf("changeColorOverLifeTime")]
        [GradientUsage(true)]
        public Gradient colorOverLifeTime;

        [Header("Mapping")]
        public SimpleSubProjectile projectilePrefab;
        public StagableProjectile logicNode;
        
        private ObjectPool<SimpleSubProjectile> _projectilePool;
        private float _currentEmissionTime;
        private float _currentEmissionCooldown;
        private Transform _emitterTransform;
        public bool ShowEmissionTime => !loop;

        private void Start()
        {
            _emitterTransform = transform;
            _projectilePool = new ObjectPool<SimpleSubProjectile>(
                () =>
                {
                    var projectile = Instantiate(projectilePrefab.gameObject, _emitterTransform.position, Quaternion.identity).GetComponent<SimpleSubProjectile>();
                    projectile.Init(_projectilePool);
                    projectile.gameObject.SetActive(false);
                    return projectile;
                },
                projectile =>
                {
                    projectile.transform.position = _emitterTransform.position;
                    projectile.Reset();
                    projectile.SetLifeTime(lifeTime);
                    projectile.SetLogicNode(logicNode);
                    projectile.Run();
                    if (changeSizeOverLifeTime) projectile.SetScaleOverLifeTime(sizeOverLifeTime);
                    if (changeColorOverLifeTime) projectile.SetColorOverLifeTime(colorOverLifeTime);
                    projectile.gameObject.SetActive(true);
                },
                projectile => projectile.gameObject.SetActive(false),
                projectile => Destroy(projectile.gameObject),
                true, poolDefaultCapacity, poolMaxCapacity
            );
        }

        private void Update()
        {
            if (useEmissionTriggerLimit && emissionTriggerLimit <= 0)
                return;
            if (!loop && _currentEmissionTime >= emissionTime)
                return;

            _currentEmissionTime += Time.deltaTime;
            _currentEmissionCooldown += Time.deltaTime;
            
            if (_currentEmissionCooldown >= emissionCooldown)
            {
                _currentEmissionCooldown = 0f;
                if (useEmissionTriggerLimit && emissionTriggerLimit > 0)
                    emissionTriggerLimit--;
                EmitSubProjectile();
            }
        }

        private void EmitSubProjectile()
        {
            for (var i = 0; i < emitCount; i++)
            {
                _projectilePool.Get();
            }
        }
    }
}