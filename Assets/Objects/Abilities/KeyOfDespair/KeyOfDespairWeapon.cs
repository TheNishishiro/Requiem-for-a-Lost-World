using System.Collections;
using Objects.Abilities.Boomerang;
using Objects.Characters;
using Objects.Stage;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.KeyOfDespair
{
    public class KeyOfDespairWeapon : PoolableWeapon<BoomerangProjectile>
    {
        [SerializeField] private GameObject slashProjectilePrefab;
        private SlashProjectile _slashProjectile;
        
        public override void Awake()
        {
            _slashProjectile = Instantiate(slashProjectilePrefab, transform).GetComponent<SlashProjectile>();
            base.Awake();
        }

        protected override bool ProjectileSpawn(BoomerangProjectile projectile)
        {
            return false;
        }

        protected override IEnumerator AttackProcess()
        {
            _slashProjectile.SetParentWeapon(this);
            _slashProjectile.transform.position = transform.position;
            _slashProjectile.SetRotation();
            _slashProjectile.gameObject.SetActive(true);
            _slashProjectile.SetNextStage();
            yield return new WaitForSeconds(WeaponStatsStrategy.GetDuplicateSpawnDelay());
        }
    }
}