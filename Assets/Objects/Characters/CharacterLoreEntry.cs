using System;
using UnityEngine;

namespace Objects.Characters
{
	[Serializable]
	public class CharacterLoreEntry
	{
		public int LevelRequirement;
		public LoreEntryType Type;
		public string Title;
		public TextAsset TextFile;
		public Sprite Background;

		public enum LoreEntryType
		{
			Text = 0,
			Dialog = 1
		}
	}
}