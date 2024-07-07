using Interfaces;
using Objects.Enemies;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Magic_Arrow
{
    public class MagicArrowProjectile : PoolableProjectile<MagicArrowProjectile>
    {
        [SerializeField] private Rigidbody projectileRigidbody;
        [SerializeField] private TrailRenderer trail;
        [SerializeField] private float rotationForce;
        [SerializeField] private float force;
        [SerializeField] private float launchForce;
        [SerializeField] private float waitBeforeHomingTime = 0.5f;
        private float _currentWaitBeforeHomingTime = 0.5f;
        private Transform _targetTransform;

        public override void SetStats(IWeaponStatsStrategy weaponStatsStrategy)
        {
            base.SetStats(weaponStatsStrategy);
            _currentWaitBeforeHomingTime = waitBeforeHomingTime;
            projectileRigidbody.AddForce(new Vector3(Random.value, Random.value, Random.value) * launchForce, ForceMode.Impulse);
        }

        private void OnTriggerEnter(Collider other)
        {
            SimpleDamage(other, true);
        }

        public void SetChaseTarget(Enemy enemy)
        {
            _targetTransform = enemy.TargetPoint;
        }

        protected override void CustomUpdate()
        {
            if (_targetTransform == null)
            {
                SetChaseTarget(EnemyManager.instance.GetRandomEnemy());
                return;
            }

            _currentWaitBeforeHomingTime -= Time.deltaTime;
            if (_currentWaitBeforeHomingTime > 0)
                return;
            
            base.CustomUpdate();
            var direction = _targetTransform.position - projectileRigidbody.position;
            direction.Normalize();
            var rotationAmount = Vector3.Cross(transform.forward, direction);
            projectileRigidbody.angularVelocity = rotationAmount * rotationForce;
            projectileRigidbody.linearVelocity = transform.forward * force;
        }
        
        public void ClearTrail()
        {
            trail.Clear();
        }
		
        private void OnEnable()
        {
            trail.emitting = true;
        }

        private void OnDisable()
        {
            trail.emitting = false;
            ClearTrail();
        }
    }
}