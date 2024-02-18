using DefaultNamespace;

namespace Events.Handlers
{
    public interface IDamageDealtHandler
    {
        void OnDamageDealt(Damageable damageable, float damage, bool isRecursion);
    }
}