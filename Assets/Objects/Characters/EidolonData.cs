using System;
using UnityEngine;

namespace Objects.Characters
{
	[Serializable]
	public class EidolonData
	{
		public Sprite EidolonTexture;
		public string EidolonName;
		[TextArea]
		public string EidolonDescription;
	}
}