using Events.Handlers;

namespace Events.Scripts
{
	public abstract class ExpPickedUpEvent : EventBase<IExpPickedUpHandler>
	{
		public static void Invoke(float amount)
		{
			for(var i = listeners.Count - 1; i >= 0; i--)
			{
				listeners[i].OnExpPickedUp(amount);
			}
		}
	}
}