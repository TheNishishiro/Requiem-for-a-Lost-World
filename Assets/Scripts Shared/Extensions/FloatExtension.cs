namespace DefaultNamespace.Extensions
{
	public static class FloatExtension
	{
		public static int ToMinutes(this float time)
		{
			return (int) (time / 60f);
		}
	}
}