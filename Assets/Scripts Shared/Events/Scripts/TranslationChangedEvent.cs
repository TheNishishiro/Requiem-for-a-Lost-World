using Events.Handlers;

namespace Events.Scripts
{
    public class TranslationChangedEvent : EventBase<ITranslationChangedHandler>
    {
        public static void Invoke()
        {
            for(var i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnTranslationChanged();
            }
        }
    }
}