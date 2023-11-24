using System;
using NaughtyAttributes;
using UnityEngine;

namespace Objects.Characters
{
	[Serializable]
	public class EidolonData
	{
		public Sprite EidolonTexture;
		public string EidolonName;
		[ResizableTextArea]
		public string EidolonDescription;
		[ResizableTextArea]
		public string EidolonQuote;
	}
}