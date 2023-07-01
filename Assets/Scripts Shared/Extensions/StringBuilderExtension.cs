using System.Text;

namespace DefaultNamespace.Extensions
{
	public static class StringBuilderExtension
	{
		public static void AppendStat(this StringBuilder sb, string name, float stat, int rarity, string appendinx = "")
		{
			if (stat == 0)
				return;
			
			var rarityFactor = 1 + ((rarity - 1) * 0.1f);
			var operation = stat > 0 ? "increase" : "decrease";
			
			sb.AppendLine($"{name} {operation} by <color=green>{stat * rarityFactor:0.00} {appendinx}</color>");
		}
	}
}