using System.Collections.Generic;
using DefaultNamespace.Events;
using Events.Handlers;

namespace Events.Scripts
{
	public abstract class DamageTakenStaticEvent : EventBase<IDamageTakenHandler>
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