using System;

namespace Data.Elements
{
	[Serializable]
	public class ElementStats
	{
		public Element element;
		public float damageReduction;

		public ElementStats()
		{
		}

		public ElementStats(ElementStats elementStats)
		{
			element = elementStats.element;
			damageReduction = elementStats.damageReduction;
		}
	}
}