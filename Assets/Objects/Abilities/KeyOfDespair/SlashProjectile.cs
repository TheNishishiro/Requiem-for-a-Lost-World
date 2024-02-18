using System;
using Managers;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.KeyOfDespair
{
    public class SlashProjectile : StagableProjectile
    {
        private int stageId = 0;

        public void SetNextStage()
        {
            stageId++;
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
            SimpleDamage(other, false);
            if (WeaponStatsStrategy.GetDamageOverTime() > 0)
                DamageOverTime(other);
        }
    }
}