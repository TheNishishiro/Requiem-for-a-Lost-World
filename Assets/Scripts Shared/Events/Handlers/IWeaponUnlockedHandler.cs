using Weapons;

namespace Events.Handlers
{
    public interface IWeaponUnlockedHandler
    {
        void OnWeaponUnlocked(WeaponBase weapon, int rarity);
    }
}