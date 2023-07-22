using Events.Handlers;

namespace Events.Scripts
{
	public abstract class SpecialBarFilledEvent : EventBase<ISpecialBarFilledHandler>
	{
		public static void Invoke()
		{
			for(var i = listeners.Count - 1; i >= 0; i--)
			{
				listeners[i].OnSpecialBarFilled();
			}
		}
	}
}