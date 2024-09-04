using System.Collections.Generic;
using UnityEngine;

namespace Objects.Enemies.EnemyWeapons
{
    [CreateAssetMenu]
    public class EnemyWeaponContainer : ScriptableObject
    {
        [SerializeField] public List<EnemyWeaponDefinition> WeaponDefinitions;
    }
}