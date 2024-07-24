using Data.Elements;
using DefaultNamespace;
using Events.Handlers;
using Objects.Enemies;

namespace Events.Scripts
{
    public class ReactionTriggeredEvent : EventBase<IReactionTriggeredEvent>
    {
        public static void Invoke(ElementalReaction reaction, Damageable damageable)
        {
            for(var i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnReactionTriggered(reaction, damageable);
            }
        }
    }
}