using System;
using Objects.Abilities;
using UnityEngine;

namespace Objects.Enemies.EnemyWeapons
{
    [Serializable]
    public class EnemyWeaponDefinition
    {
        [SerializeField] public GameObject spawnPrefab;
        [SerializeField] public EnemyWeaponId weaponId;
        [SerializeField] public WeaponStats weaponStats;
    }
}