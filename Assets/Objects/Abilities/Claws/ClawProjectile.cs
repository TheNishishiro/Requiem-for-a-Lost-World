using UnityEngine;
using Weapons;

namespace Objects.Abilities.Claws
{
    public class ClawProjectile : StagableProjectile
    {
        private ClawWeapon ClawWeapon => (ClawWeapon)ParentWeapon;
        public bool IsLastStage => stageId == 2;
        private int stageId = 0;

        public void SetNextStage()
        {
            stageId++;
            ProjectileDamageIncreasePercentage = IsLastStage && ClawWeapon.BeastsWrath ? 1.5f : 0;
            if (stageId > 2)
                stageId = 0;
        }

        public void SetRotation()
        {
            var zRotationByStage = stageId switch
            {
                0 => 45,
                1 => -45,
                _ => 0
            };
            transformCache.localRotation = Quaternion.Euler(0, 0, zRotationByStage); 
        }

        protected override void Destroy()
        {
            gameObject.SetActive(false);
        }

        public void OnTriggerEnter(Collider other)
        {
            SimpleDamage(other, false, false, out var damageable);
            DamageOverTime(damageable, other);
            ApplyVulnerability(damageable, other, 2);
        }
    }
}