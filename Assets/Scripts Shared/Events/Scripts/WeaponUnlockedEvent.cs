using Events.Handlers;
using Weapons;

namespace Events.Scripts
{
    public class WeaponUnlockedEvent : EventBase<IWeaponUnlockedHandler>
    {
        public static void Invoke(WeaponBase weapon, int rarity)
        {
            for(var i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnWeaponUnlocked(weapon, rarity);
            }
        }
    }
}