using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Managers;
using Objects.Abilities.Magic_Ball;
using Objects.Enemies;
using UnityEngine;
using UnityEngine.Pool;
using Weapons;

public class FireBallWeapon : PoolableWeapon<FireBallProjectile>
{
    [SerializeField] private AudioClip fireballSpawnSound;

    protected override bool ProjectileSpawn(FireBallProjectile projectile)
    {
        var closestTarget = Utilities.FindClosestDamageable(transform.position, FindObjectsByType<Damageable>(FindObjectsSortMode.None), out var distanceToClosest);
        if (closestTarget is null)
            return false;
            
        var transform1 = closestTarget.targetPoint.transform;
        var position = transform1.position;
        projectile.transform.position = transform.position;
        projectile.SetStats(weaponStats);
        projectile.SetDirection(position.x, position.y, position.z);
        return true;
    }

    public override bool IsUnlocked(SaveFile saveFile)
    {
        return saveFile.IsAchievementUnlocked(AchievementEnum.Survive15MinutesWithChitose);
    }
}
