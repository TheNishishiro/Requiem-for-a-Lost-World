using System.Collections.Generic;

namespace Events
{
	public class EventBase<T>
	{
		protected static List<T> listeners = new ();

		public static void Register(T listener)
		{
			listeners.Add(listener);
		}

		public static void Unregister(T listener)
		{
			listeners.Remove(listener);
		}
		
	}
}