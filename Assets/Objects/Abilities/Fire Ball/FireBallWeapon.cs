using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Managers;
using Objects.Abilities.Magic_Ball;
using Objects.Enemies;
using UnityEngine;
using Weapons;

public class FireBallWeapon : WeaponBase
{
    [SerializeField] private AudioClip fireballSpawnSound;
    
    public override void Attack()
    {
        var currentPosition = transform.position;
        var closestTarget = Utilities.FindClosestDamageable(currentPosition, FindObjectsByType<Damageable>(FindObjectsSortMode.None), out var distanceToClosest);
        if (closestTarget is null)
            return;

        var fireBall = SpawnManager.instance.SpawnObject(currentPosition, spawnPrefab);
        var projectileComponent = fireBall.GetComponent<FireBallProjectile>();
        projectileComponent.SetStats(weaponStats);
        var transform1 = closestTarget.transform;
        var position = transform1.position;
        projectileComponent.SetParentWeapon(this);
        projectileComponent.SetDirection(position.x, position.y, position.z);
        //AudioSource.PlayClipAtPoint(fireballSpawnSound, currentPosition, 0.2f);
    }

    public override bool IsUnlocked(SaveFile saveFile)
    {
        return saveFile.IsAchievementUnlocked(AchievementEnum.Survive15MinutesWithChitose);
    }
}
