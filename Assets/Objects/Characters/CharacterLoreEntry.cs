using System;
using UI.Main_Menu.Story_Layout_Panel;
using UnityEngine;

namespace Objects.Characters
{
	[Serializable]
	public class CharacterLoreEntry : LoreEntry
	{
		public int Chapter;
		public int LevelRequirement;
	}
}