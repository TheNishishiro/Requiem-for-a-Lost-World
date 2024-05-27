using Events.Handlers;

namespace Events.Scripts
{
    public class EnemyImmobileEvent : EventBase<IEnemyImmobileHandler>
    {
        public static void Invoke(float time)
        {
            for (var i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEnemyStunned(time);
            }
        }
    }
}