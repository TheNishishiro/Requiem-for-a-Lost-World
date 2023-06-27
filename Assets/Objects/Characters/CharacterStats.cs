using DefaultNamespace.Extensions;

namespace Objects.Characters
{
	public class CharacterStats
	{
		public CharacterStats(string name, string value)
		{
			Name = name;
			Value = value;
		}
		
		public CharacterStats(string name, object value, bool isPercentage = false, bool isInvertedColor = false)
		{
			Name = name;
			var floatValue = float.Parse(value.ToString());
			var color = floatValue < 0 && !isInvertedColor ? "red" : "green";
			if (floatValue == 0)
				color = "white";
			var stringValue = isPercentage ? floatValue.ToPercentage() : value.ToString();
			Value = $"<color={color}>{stringValue}</color>";
		}

		public string Name { get; set; }
		public string Value { get; set; }
	}
}