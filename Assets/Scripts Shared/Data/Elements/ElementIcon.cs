using System;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data.Elements
{
	[Serializable]
	public class ElementIcon
	{
		public Element element;
		public Sprite icon;
	}
}