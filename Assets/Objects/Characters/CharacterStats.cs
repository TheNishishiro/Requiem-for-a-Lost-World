﻿using DefaultNamespace.Extensions;

namespace Objects.Characters
{
	public class CharacterStats
	{
		public CharacterStats(string name, string value)
		{
			Name = name;
			Value = value;
		}
		
		public CharacterStats(string name, object value, bool isPercentage = false, bool isInvertedColor = false, float baseValue = 0)
        {
            Name = name;
            var floatValue = float.Parse(value.ToString());
            var color = "white";
            if (baseValue > floatValue)
	            color = isInvertedColor ? "green" : "red";
            else if (baseValue < floatValue)
	            color = isInvertedColor ? "red" : "green";
        
            var stringValue = isPercentage ? floatValue.ToPercentage() : value.ToString();
        
            Value = $"<color={color}>{stringValue}</color>";
        }

		public string Name { get; set; }
		public string Value { get; set; }
	}
}