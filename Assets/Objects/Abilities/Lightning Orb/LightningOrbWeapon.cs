using System.Collections;
using Managers;
using Objects.Abilities;
using UnityEngine;
using Weapons;

public class LightningOrbWeapon : PoolableWeapon<LightningOrbProjectile>
{
    private const float RotationStep = 70;
    private float _rotateOffset = 0;
    
    public override void SetupProjectile(NetworkProjectile networkProjectile)
    {
        var tCache = transform;
        var position = tCache.position + new Vector3(WeaponStatsStrategy.GetScale() + 1,0,0);
    
        networkProjectile.Initialize(this, position);
        networkProjectile.transform.RotateAround(tCache.position, Vector3.up, _rotateOffset);
        _rotateOffset += RotationStep;
    }
    
    protected override IEnumerator AttackProcess()
    {
        OnAttackStart();
			
        for (var i = 0; i < WeaponStatsStrategy.GetAttackCount(); i++)
        {
            Attack();
        }

        OnAttackEnd();
        yield break;
    }
}
