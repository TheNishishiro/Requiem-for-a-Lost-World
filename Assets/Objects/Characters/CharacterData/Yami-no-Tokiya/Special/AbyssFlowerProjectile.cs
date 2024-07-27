using System;
using Data.Elements;
using Interfaces;
using Managers;
using Objects.Enemies;
using Objects.Stage;
using UnityEngine;
using UnityEngine.Pool;
using Weapons;
using Random = UnityEngine.Random;

namespace Objects.Characters.Yami_no_Tokiya.Special
{
    public class AbyssFlowerProjectile :  PoolableProjectile<AbyssFlowerProjectile>
    {
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player") && TimeAlive < 5)
            {
                Pickup();
            }
            
            if (TimeAlive < 5) return;
            if (!other.CompareTag("Enemy")) return;
            
            DamageArea(other, out _, true);
        }

        public void Pickup()
        {
            SpecialBarManager.instance.Increment();
            Destroy();
        }
    }
}