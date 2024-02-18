using DefaultNamespace;
using Events.Handlers;

namespace Events.Scripts
{
    public class DamageDealtEvent : EventBase<IDamageDealtHandler>
    {
        public static void Invoke(Damageable damageable, float damage, bool isRecursion)
        {
            for(var i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnDamageDealt(damageable, damage, isRecursion);
            }
        }
    }
}