using Data.Elements;
using DefaultNamespace.Data.Weapons;
using Objects.Abilities;
using UnityEngine;

namespace Interfaces
{
    public interface IWeapon
    {
        IWeaponStatsStrategy GetWeaponStrategy();
        Transform GetTransform();
        bool IsUseNetworkPool();
        int GetId();
        Element GetElement();
        int GetInstanceID();
        GameObject GetProjectile();
    }
}