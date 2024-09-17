using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Managers;
using Objects.Abilities;
using Objects.Abilities.Magic_Ball;
using Objects.Enemies;
using UnityEngine;
using Weapons;

public class DigitalArrowWeapon : PoolableWeapon<DigitalArrowProjectile>
{
    public bool IsAlgorithmicCascade;
    
    public override void SetupProjectile(NetworkProjectile networkProjectile)
    {
        var closestTarget = Utilities.FindClosestEnemy(transform.position, EnemyManager.instance.GetActiveEnemies(), out var distanceToClosest);
        if (closestTarget == null)
        {
            networkProjectile.Despawn(WeaponId);
            return;
        }

        networkProjectile.Initialize(this, transform.position);
        var position = closestTarget.TargetPoint.position;

        var projectile = networkProjectile.GetProjectile<DigitalArrowProjectile>();
        projectile.SetDirection(position.x, position.y, position.z);
        projectile.ClearTrail();
    }

    protected override void OnLevelUp()
    {
        if (LevelField >= 8)
            IsAlgorithmicCascade = true;
    }
}
