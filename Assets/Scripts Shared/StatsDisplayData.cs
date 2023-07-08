using DefaultNamespace.Extensions;

public class StatsDisplayData
{
	public StatsDisplayData(string name, string value)
	{
		Name = name;
		Value = value;
	}
		
	public StatsDisplayData(string name, object value, string description = null, bool isPercentage = false, bool isInvertedColor = false, float baseValue = 0)
	{
		Name = name;
		Description = description;
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
	public string Description { get; set; }
}