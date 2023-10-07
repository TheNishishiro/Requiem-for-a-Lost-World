using System;
using UnityEngine;

namespace UI.Main_Menu.Story_Layout_Panel
{
	[Serializable]
	public class LoreEntry
	{
		public TextAsset TextFile;
		public Sprite Background;
		public int ChapterNumber => int.Parse(TextFile.name.Split('_')[1]);
		public int EntryNumber => int.Parse(TextFile.name.Split('_')[3]);
	}
}