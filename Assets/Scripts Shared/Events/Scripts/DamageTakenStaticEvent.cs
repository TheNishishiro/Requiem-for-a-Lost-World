using System.Collections.Generic;
using Events.Handlers;

namespace Events.Scripts
{
	public static class DamageTakenStaticEvent
	{
		private static List<IDamageTakenHandler> listeners = new ();

		public static void Register(IDamageTakenHandler listener)
		{
			listeners.Add(listener);
		}

		public static void Unregister(IDamageTakenHandler listener)
		{
			listeners.Remove(listener);
		}

		public static void Invoke(float damage)
		{
			for(var i = listeners.Count - 1; i >= 0; i--)
			{
				listeners[i].OnDamageTaken(damage);
			}
		}
	}
}