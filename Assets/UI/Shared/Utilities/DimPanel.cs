using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Shared.Utilities
{
	public class DimPanel : MonoBehaviour
	{
		public Image color;
		public float dimAlpha = 100f;
		public float originalAlpha = 200f;
		
		public void SetDim(bool isDimmed)
		{
			var alpha = (isDimmed ? dimAlpha : originalAlpha) / 255f;
			color.color = new Color(color.color.r,color.color.g,color.color.b,alpha);
		}
	}
}