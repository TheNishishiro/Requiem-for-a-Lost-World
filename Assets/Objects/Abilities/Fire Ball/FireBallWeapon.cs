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

    protected override bool ProjectileSpawn(FireBallProjectile projectile)
    {
        var closestTarget = Utilities.FindClosestEnemy(transform.position, EnemyManager.instance.GetActiveEnemies(), out var distanceToClosest);
        if (closestTarget is null)
            return false;
            
        var position = closestTarget.TargetPoint.position;
        projectile.transform.position = transform.position;
        projectile.SetParentWeapon(this);
        projectile.SetDirection(position.x, position.y, position.z);
        return true;
    }

    public override void OnEnemyKilled()
    {
        if (!GameData.IsCharacterWithRank(CharactersEnum.Chitose, CharacterRank.E2)) return;
        if (enemiesKilled++ <= 20) return;
        
        weaponStats.DamageIncreasePercentage += 0.01f;
        enemiesKilled = 0;
    }
    
    
}
