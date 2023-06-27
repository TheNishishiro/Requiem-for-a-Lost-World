namespace DefaultNamespace.Extensions
{
	public static class StringExtension
	{
		public static string ToPercentage(this float value)
		{
			return $"{value * 100:0.00}%";
		}
	}
}