using System;
using System.Collections.Generic;
using System.Linq;
using Objects.Enemies.EnemyWeapons.Weapon_Prefabs;
using UnityEngine;
using Weapons;

namespace Objects.Enemies.EnemyWeapons
{
    public class EnemyWeaponManager : MonoBehaviour
    {
        public static EnemyWeaponManager instance;
        [SerializeField] private EnemyWeaponContainer weaponContainer;
        private Dictionary<EnemyWeaponId, EnemyWeaponDefinition> _dictionary;
        
        private void Awake()
        {
            if (instance == null)
                instance = this;

            _dictionary = weaponContainer.WeaponDefinitions.ToDictionary(x => x.weaponId, x => x);
        }

        public EnemyWeapon GetWeapon(EnemyWeaponId weaponId)
        {
            var definition = _dictionary[weaponId];
            switch (weaponId)
            {
                case EnemyWeaponId.Fireball:
                    return new EnemyFireballWeapon(definition.spawnPrefab, definition.weaponStats, definition.weaponId);
                default:
                    throw new ArgumentOutOfRangeException(nameof(weaponId));
            }
        }
    }
}