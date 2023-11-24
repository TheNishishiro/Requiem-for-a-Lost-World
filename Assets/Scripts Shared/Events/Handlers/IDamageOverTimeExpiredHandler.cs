using DefaultNamespace;
using Interfaces;

namespace Events.Handlers
{
	public interface IDamageOverTimeExpiredHandler
	{
		void OnDoTExpired(Damageable damageable, float damage);
	}
}