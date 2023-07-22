using System.Collections.Generic;
using Events.Handlers;

namespace Events.Scripts
{
	public abstract class DamageTakenEvent : EventBase<IDamageTakenHandler>
	{
		public static void Invoke(float damage)
		{
			for(var i = listeners.Count - 1; i >= 0; i--)
			{
				listeners[i].OnDamageTaken(damage);
			}
		}
	}
}