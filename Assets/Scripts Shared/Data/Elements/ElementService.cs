using System;
using UnityEngine;

namespace Data.Elements
{
	public static class ElementService
	{
		public static Color ElementToColor(Element? element)
		{
			var color = Color.white;
			if (element == null)
				return color;
			
			switch (element)
			{
				case Element.Disabled:
					// remaining white
					break;
				case Element.Unstable:
					// remaining white
					break;
				case Element.Fire:
					color = new Color(1, 0.5f, 0); // orange
					break;
				case Element.Lightning:
					color = new Color(0, 0.5f, 1); // light blue
					break;
				case Element.Ice:
					color = new Color(0.5f, 0.8f, 1); // light cyan
					break;
				case Element.Physical:
					color = new Color(0.6f, 0.3f, 0.2f); // brown
					break;
				case Element.Wind:
					color = new Color(0.6f, 1, 0.4f); // light green
					break;
				case Element.Light:
					color = new Color(1, 1, 0.5f); // yellow
					break;
				case Element.Cosmic:
					color = new Color(0.5f, 0, 0.5f); // purple
					break;
				case Element.Earth:
					color = new Color(0, 0.5f, 0); // dark green
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(element), element, null);
			}

			return color;
		}
	}
}