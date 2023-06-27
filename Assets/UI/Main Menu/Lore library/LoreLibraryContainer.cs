using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Data;
using Objects.Characters;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace UI.Main_Menu.Lore_library
{
	public class LoreLibraryContainer : MonoBehaviour
	{
		[SerializeField] private LoreEntry entryPrefab;
		[SerializeField] private LoreEntryPanel loreEntryPanel;
		private List<LoreEntry> _entries;
		
		public void Refresh(CharacterData characterData, CharacterSaveData characterSaveData)
		{
			_entries ??= new List<LoreEntry>();
			foreach(var loreEntry in characterData.loreEntries.Where(x => x.LevelRequirement <= characterSaveData.Level))
			{
				var loreEntryObject = Instantiate(entryPrefab, transform);
				loreEntryObject.SetEntry(loreEntry);
				_entries.Add(loreEntryObject);
			}
		}

		public void Clear()
		{
			_entries ??= new List<LoreEntry>();
			foreach (var entry in _entries)
			{
				Destroy(entry.gameObject);
			}
		}

		public void OpenEntryPanel(CharacterLoreEntry loreEntry)
		{
			loreEntryPanel.Open(loreEntry);
		}
	}
}