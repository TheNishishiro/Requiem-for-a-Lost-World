using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Managers;
using Objects.Abilities.Magic_Ball;
using Objects.Enemies;
using UnityEngine;
using Weapons;

public class MagicBallWeapon : WeaponBase
{
    public override void Attack()
    {
        var closestTarget = Utilities.FindClosestDamageable(transform.position, FindObjectsOfType<Damageable>(), out var distanceToClosest);
        if (closestTarget is null)
            return;
        
        var magicBall = SpawnManager.instance.SpawnObject(transform.position, spawnPrefab);
        var projectileComponent = magicBall.GetComponent<MagicBallProjectile>();
        projectileComponent.SetStats(weaponStats);
        var transform1 = closestTarget.transform;
        var position = transform1.position;
        projectileComponent.SetParentWeapon(this);
        projectileComponent.SetDirection(position.x, position.y, position.z);
    }
}
