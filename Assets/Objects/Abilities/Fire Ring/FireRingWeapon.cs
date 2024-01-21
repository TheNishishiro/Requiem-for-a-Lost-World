using UnityEngine;
using Weapons;

namespace Objects.Abilities.Fire_Ring
{
    public class FireRingWeapon : PoolableWeapon<FireRingProjectile>
    {
        public bool IsRingOfConflagration { get; set; }
        public bool IsSearingHeat  { get; set; }
        public bool IsInfernoTrail   { get; set; }
        private int _enemiesKilled = 0;
        
        protected override bool ProjectileSpawn(FireRingProjectile projectile)
        {
            projectile.transform.position = transform.position;
            projectile.SetParentWeapon(this);
            return true;
        }

        protected override void OnLevelUp()
        {
            switch (LevelField)
            {
                case 7:
                    IsSearingHeat = true;
                    break;
                case 10:
                    IsInfernoTrail = true;
                    break;
                case 11:
                    IsRingOfConflagration = true;
                    break;
            }
        }

        public override void OnEnemyKilled()
        {
            if (!IsRingOfConflagration) return;
            _enemiesKilled++;
            if (_enemiesKilled < 100) return;
            _enemiesKilled = 0;
            var number = Random.Range(0, 3);
            switch (number)
            {
                case 0:
                    weaponStats.DamageOverTimeDuration += 0.01f;
                    break;
                case 1:
                    weaponStats.DamageOverTime += 0.01f;
                    break;
            }
            
            base.OnEnemyKilled();
        }
    }
}