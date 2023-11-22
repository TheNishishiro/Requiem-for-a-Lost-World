using Events.Handlers;
using Objects.Players.PermUpgrades;

namespace Events.Scripts
{
    public abstract class StatChangedEvent : EventBase<IStatChangedHandler>
    {
        public static void Invoke(StatEnum stat, float value)
        {
            for(var i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnStatChanged(stat, value);
            }
        }
    }
}