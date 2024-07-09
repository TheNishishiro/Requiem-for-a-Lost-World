using System;
using UnityEngine;
using UnityEngine.Pool;
using Weapons;

namespace Objects.Abilities
{
    public class SimpleSubProjectile : MonoBehaviour
    {
        private float _lifeTime;
        private float _maxLifeTime;
        private Vector3 _baseScale;
        private ObjectPool<SimpleSubProjectile> _parentPool;
        private AnimationCurve _scaleOverLifeTime;
        private bool _useScaleOverLifeTime;
        private Transform _transform;
        private StagableProjectile _logicNode;
        private bool _isInitialized;
        private Material _particleMaterial;
        private bool _isColorOverLifeTime;
        [GradientUsage(true)]
        private Gradient _colorOverLifeTime;
        private static readonly int MaterialColorProperty = Shader.PropertyToID("_Color");

        private void Awake()
        {
            _baseScale = transform.localScale;
            transform.localScale = Vector3.zero;
            _particleMaterial = GetComponentInChildren<MeshRenderer>().material;
        }

        public void Init(ObjectPool<SimpleSubProjectile> projectilePool)
        {
            _parentPool = projectilePool;
            _transform = transform;
        }

        private void Update()
        {
            if (!_isInitialized)
                return;
            
            if (_lifeTime <= 0)
            {
                _parentPool.Release(this);
                return;
            }

            _lifeTime -= Time.deltaTime;
            SetScaleByLifeTime();
            SetColorByLifeTime();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_isInitialized) return;
            if (!_logicNode.gameObject.activeSelf) return;
            
            _logicNode.SubProjectileTrigger(this, other);
        }

        public void Reset()
        {
            _isInitialized = false;
            _useScaleOverLifeTime = false;
            _lifeTime = _maxLifeTime;
            transform.localScale = Vector3.zero;
            //if (_transform != null) 
            //    _transform.localScale = _baseScale;
        }

        public void Run()
        {
            _isInitialized = true;
            SetScaleByLifeTime();
            SetColorByLifeTime();
        }

        private void SetScaleByLifeTime()
        {
            if (!_useScaleOverLifeTime) return;
            _transform.localScale = _baseScale * _scaleOverLifeTime.Evaluate(_lifeTime / _maxLifeTime);
        }

        private void SetColorByLifeTime()
        {
            if (!_useScaleOverLifeTime) return;
            _particleMaterial.SetColor(MaterialColorProperty, _colorOverLifeTime.Evaluate(1 - (_lifeTime / _maxLifeTime)));
        }

        public void SetLifeTime(float lifeTime)
        {
            _lifeTime = _maxLifeTime = lifeTime;
        }

        public void SetScaleOverLifeTime(AnimationCurve scaleOverLifeTime)
        {
            _scaleOverLifeTime = scaleOverLifeTime;
            _useScaleOverLifeTime = true;
        }

        public void SetColorOverLifeTime(Gradient colorOverLifeTime)
        {
            _isColorOverLifeTime = true;
            _colorOverLifeTime = colorOverLifeTime;
        }

        public void SetLogicNode(StagableProjectile logicNode)
        {
            _logicNode = logicNode;
        }
    }
}