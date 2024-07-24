using Events.Handlers;
using Objects.Enemies;

namespace Events.Scripts
{
    public class EnemyAttackEvent : EventBase<IEnemyAttackEvent>
    {
        public static void Invoke(Enemy enemy)
        {
            for(var i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEnemyAttack(enemy);
            }
        }
    }
}