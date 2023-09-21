using DefaultNamespace;
using Events.Handlers;

namespace Events.Scripts
{
	public class DamageOverTimeExpiredHandler : EventBase<IDamageOverTimeExpiredHandler>
	{
		public static void Invoke(Damageable damageable)
		{
			for(var i = listeners.Count - 1; i >= 0; i--)
			{
				listeners[i].OnDoTExpired(damageable);
			}
		}
	}
}