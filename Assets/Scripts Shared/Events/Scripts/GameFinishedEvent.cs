using Events.Handlers;

namespace Events.Scripts
{
    public class GameFinishedEvent : EventBase<IGameFinishedHandler>
    {
        public static void Invoke(bool isWin)
        {
            for(var i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnGameFinished(isWin);
            }
        }
    }
}