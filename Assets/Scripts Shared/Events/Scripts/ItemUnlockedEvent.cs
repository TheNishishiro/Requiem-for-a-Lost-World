using Events.Scripts;
using Objects.Items;

namespace Events.Handlers
{
    public class ItemUnlockedEvent : EventBase<IItemUnlockedHandler>
    {
        public static void Invoke(ItemBase item, int rarity)
        {
            for(var i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnWeaponUnlocked(item, rarity);
            }
        }
    }
}