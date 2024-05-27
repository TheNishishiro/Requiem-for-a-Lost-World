using Objects.Items;

namespace Events.Scripts
{
    public interface IItemUnlockedHandler
    {
        void OnWeaponUnlocked(ItemBase item, int rarity);
    }
}