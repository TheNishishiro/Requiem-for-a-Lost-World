using System;
using Data.Elements;
using Interfaces;
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
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Enemy")) return;
            
            SimpleDamage(other, false, true, out _);
        }
    }
}