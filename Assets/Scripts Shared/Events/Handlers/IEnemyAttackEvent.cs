using Objects.Enemies;

namespace Events.Handlers
{
    public interface IEnemyAttackEvent
    {
        void OnEnemyAttack(Enemy enemy);
    }
}