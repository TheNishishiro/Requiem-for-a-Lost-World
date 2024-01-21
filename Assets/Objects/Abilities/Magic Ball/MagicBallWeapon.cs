using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Managers;
using Objects.Abilities.Magic_Ball;
using Objects.Enemies;
using UnityEngine;
using Weapons;

public class MagicBallWeapon : PoolableWeapon<MagicBallProjectile>
{
    protected override bool ProjectileSpawn(MagicBallProjectile projectile)
    {
        var closestTarget = Utilities.FindClosestEnemy(transform.position, EnemyManager.instance.GetActiveEnemies(), out var distanceToClosest);
        if (closestTarget == null)
            return false;

        projectile.transform.position = transform.position;
        projectile.SetParentWeapon(this);
        var position = closestTarget.TargetPoint.position;
        projectile.SetDirection(position.x, position.y, position.z);
        projectile.ClearTrail();
        return true;
    }
}
