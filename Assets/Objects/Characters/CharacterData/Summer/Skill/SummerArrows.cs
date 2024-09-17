using System;
using Data.Elements;
using DefaultNamespace;
using Objects;
using Objects.Abilities;
using Objects.Characters;
using Objects.Players.Scripts;
using Objects.Stage;
using UnityEngine;

public class SummerArrows : MonoBehaviour
{
    private ElementalWeapon _elementalWeapon;

    private void Start()
    {
        var weaponStats = new WeaponStats()
        {
            Damage = 30
        };
        _elementalWeapon = new ElementalWeapon(Element.Physical)
        {
            WeaponStatsStrategy = new WeaponStatsStrategyBase(weaponStats, Element.Physical)
            
        };
        _elementalWeapon.WeaponStatsStrategy.GetDamageDealt();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (!other.CompareTag("Enemy")) return;
        var damagable = other.GetComponent<Damageable>();
        if (GameData.IsCharacterRank(CharacterRank.E2))
            damagable.SetVulnerable(WeaponEnum.Unset, 2f, 1f);
        damagable .TakeDamage(_elementalWeapon.WeaponStatsStrategy.GetDamageDealt(), _elementalWeapon);
    }
}
