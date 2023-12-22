using System;
using Objects.Characters;

namespace Objects.Players.PermUpgrades
{
	public class StatTypeAttribute : Attribute
	{
		public bool IsPercent { get; protected set; }
		
		public StatTypeAttribute(bool isPercent)
		{
			IsPercent = isPercent;
		}
	}
	
	public static class StatTypeValueExtension
	{
		public static bool IsPercent(this PermUpgradeType statTypeEnum)
		{
			var fieldInfo = statTypeEnum.GetType().GetField(statTypeEnum.ToString());
			var attributes = fieldInfo.GetCustomAttributes(typeof(StatTypeAttribute), false) as StatTypeAttribute[];
			return attributes?.Length > 0 && attributes[0].IsPercent;
		}
		public static bool IsPercent(this StatEnum statTypeEnum)
		{
			var fieldInfo = statTypeEnum.GetType().GetField(statTypeEnum.ToString());
			var attributes = fieldInfo.GetCustomAttributes(typeof(StatTypeAttribute), false) as StatTypeAttribute[];
			return attributes?.Length > 0 && attributes[0].IsPercent;
		}
	}
}