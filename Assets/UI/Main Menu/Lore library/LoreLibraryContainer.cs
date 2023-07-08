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
		[SerializeField] private LoreChapterPanel loreChapterPanelPrefab;
		[SerializeField] private LoreChapterContainer chapterEntryContainerPrefab;
		private List<GameObject> _entries;
		
		public void Refresh(CharacterData characterData, CharacterSaveData characterSaveData)
		{
			_entries ??= new List<GameObject>();
			characterData.loreEntries
				.Where(x => x.LevelRequirement <= characterSaveData.Level)
				.GroupBy(x => x.Chapter)
				.ToList()
				.ForEach(CreateChapter);
		}

		private void CreateChapter(IGrouping<int, CharacterLoreEntry> characterLoreEntries)
		{
			var chapterPanel = Instantiate(loreChapterPanelPrefab, transform);
			chapterPanel.SetTitle($"Chapter {characterLoreEntries.Key}");
			_entries.Add(chapterPanel.gameObject);
			var chapterContainer = Instantiate(chapterEntryContainerPrefab, transform);
			chapterContainer.gameObject.SetActive(false);
			_entries.Add(chapterContainer.gameObject);
			chapterPanel.SetContainer(chapterContainer);
			
			foreach(var loreEntry in characterLoreEntries)
			{
				chapterContainer.AddEntry(loreEntry);
			}
		}

		public void Clear()
		{
			_entries ??= new List<GameObject>();
			foreach (var entry in _entries)
			{
				Destroy(entry.gameObject);
			}
		}
	}
}