using System;
using Interfaces;
using UnityEngine;
using Weapons;

namespace Objects.Characters.Nishi_HoF.Skill
{
    public class LanceProjectile : PoolableProjectile<LanceProjectile>
    {
        private void OnTriggerEnter(Collider other)
        {
            SimpleFollowUpDamage(other);
        }
    }
}