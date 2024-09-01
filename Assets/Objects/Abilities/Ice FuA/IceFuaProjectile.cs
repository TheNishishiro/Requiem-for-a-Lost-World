using Objects.Enemies;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Ice_FuA
{
    public class IceFuaProjectile : PoolableProjectile<IceFuaProjectile>
    {
        [SerializeField] private Rigidbody projectileRigidbody;
        [SerializeField] private float rotationForce;
        [SerializeField] private float force;
        [SerializeField] private float launchForce;
        [SerializeField] private float waitBeforeHomingTime = 0.5f;
        private float _currentWaitBeforeHomingTime = 0.5f;
        private Enemy _target;
        
        public void SetTarget(Enemy enemy)
        {
            _target = enemy;
            _currentWaitBeforeHomingTime = waitBeforeHomingTime;
            projectileRigidbody.AddForce(new Vector3(Random.value, Random.value, Random.value) * launchForce, ForceMode.Impulse);
            CurrentTimeToLive = GetTimeToLive() - 1.5f;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            SimpleDamage(other, true);
        }
        
        protected override void CustomUpdate()
        {
            _currentWaitBeforeHomingTime -= Time.deltaTime;
            if (_currentWaitBeforeHomingTime > 0)
                return;

            if (!_target || _target.IsDying())
            {
                OnLifeTimeEnd();
                return;
            }

            base.CustomUpdate();
            var direction = _target.TargetPoint.position - projectileRigidbody.position;
            direction.Normalize();
            var rotationAmount = Vector3.Cross(transform.forward, direction);
            projectileRigidbody.angularVelocity = rotationAmount * rotationForce;
            projectileRigidbody.linearVelocity = transform.forward * force;
        }
    }
}