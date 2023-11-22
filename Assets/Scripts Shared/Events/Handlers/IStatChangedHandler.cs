using Objects.Players.PermUpgrades;

namespace Events.Handlers
{
    public interface IStatChangedHandler
    {
        void OnStatChanged(StatEnum stat, float value);
    }
}