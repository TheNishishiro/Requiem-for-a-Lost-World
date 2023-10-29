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
        var closestTarget = Utilities.FindClosestDamageable(transform.position, FindObjectsByType<Damageable>(FindObjectsSortMode.None), out var distanceToClosest);
        if (closestTarget == null)
            return false;

        projectile.transform.position = transform.position;
        projectile.SetStats(weaponStats);
        var transform1 = closestTarget.transform;
        var position = transform1.position;
        projectile.SetDirection(position.x, position.y, position.z);
        return true;
    }
}
