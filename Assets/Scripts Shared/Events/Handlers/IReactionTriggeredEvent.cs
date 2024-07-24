using Data.Elements;
using DefaultNamespace;

namespace Events.Handlers
{
    public interface IReactionTriggeredEvent
    {
        void OnReactionTriggered(ElementalReaction reaction, Damageable damageable);
    }
}