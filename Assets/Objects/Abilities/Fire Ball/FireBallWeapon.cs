using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Managers;
using Objects.Abilities;
using Objects.Abilities.Magic_Ball;
using Objects.Characters;
using Objects.Enemies;
using Objects.Stage;
using UnityEngine;
using UnityEngine.Pool;
using Weapons;

public class FireBallWeapon : PoolableWeapon<FireBallProjectile>
{
    [SerializeField] private AudioClip fireballSpawnSound;
    private int enemiesKilled = 0;

    public override void SetupProjectile(NetworkProjectile networkProjectile)
    {
        var closestTarget = Utilities.FindClosestEnemy(transform.position, EnemyManager.instance.GetActiveEnemies(), out var distanceToClosest);
        if (closestTarget is null)
        {
            networkProjectile.Despawn(WeaponId);
            return;
        }

        var position = closestTarget.TargetPoint.position;
        
        networkProjectile.Initialize(this, transform.position);
        networkProjectile.GetProjectile<FireBallProjectile>().SetDirection(position.x, position.y, position.z);
        networkProjectile.projectile.gameObject.SetActive(true);
    }

    protected override int GetAttackCount()
    {
        if (GameData.IsCharacter(CharactersEnum.Chitose))
            return base.GetAttackCount() + 2;
        return base.GetAttackCount();
    }

    public override void OnEnemyKilled()
    {
        if (!GameData.IsCharacterWithRank(CharactersEnum.Chitose, CharacterRank.E2)) return;
        if (enemiesKilled++ <= 20) return;
        
        weaponStats.DamageIncreasePercentage += 0.01f;
        enemiesKilled = 0;
    }
    
    
}
