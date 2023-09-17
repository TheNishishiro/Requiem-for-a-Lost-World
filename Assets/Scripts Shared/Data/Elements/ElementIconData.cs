using System.Collections.Generic;
using UnityEngine;

namespace Data.Elements
{
	[CreateAssetMenu]
	public class ElementIconData : ScriptableObject
	{
		public List<ElementIcon> elementIcons;
		
		public Sprite GetIcon(Element element)
		{
			return elementIcons.Find(x => x.element == element).icon;
		}
	}
}