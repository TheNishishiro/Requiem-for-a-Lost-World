using System.Collections.Generic;
using UnityEngine;

namespace UI.Main_Menu.Character_List_Menu
{
	public class StatsScrollMenuPanel : MonoBehaviour
	{
		[SerializeField] private GameObject statsMenuEntryPrefab;
		[SerializeField] private GameObject statsMenuEntryContainer;
		private List<GameObject> _statsMenuEntries;

		public void AddEntry(string entryName, string value)
		{
			_statsMenuEntries ??= new List<GameObject>();
			var entry = Instantiate(statsMenuEntryPrefab, statsMenuEntryContainer.transform);
			entry.GetComponent<StatsMenuEntry>().Setup(entryName, value);
			_statsMenuEntries.Add(entry);
		}
		
		public void ClearEntries()
		{
			_statsMenuEntries ??= new List<GameObject>();
			foreach (var entry in _statsMenuEntries)
			{
				Destroy(entry.gameObject);
			}
			
			_statsMenuEntries.Clear();
		}
	}
}