using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.Data.Game;
using Interfaces;
using Managers;
using Objects.Enemies.EnemyWeapons;
using UnityEngine;

namespace Objects.Enemies
{
    public class EnemyWeaponComponent : MonoBehaviour
    {
        [SerializeField] private ChaseComponent chaseComponent;
        [SerializeField] private Enemy ownerEnemy;
        private readonly List<EnemyWeapon> _weapon = new ();

        public void Clear()
        {
            _weapon.Clear();
        }
        
        public void SetWeapon(EnemyWeapon weapon)
        {
            weapon.InitWeapon(ownerEnemy.TargetPoint);
            weapon.SetChaseComponent(chaseComponent);
            _weapon.Add(weapon);
        }

        public void Update()
        {
            _weapon.ForEach(x =>
            {
                if (x.WeaponUpdate())
                    StartCoroutine(x.AttackProcess());
            });
        }
    }
}