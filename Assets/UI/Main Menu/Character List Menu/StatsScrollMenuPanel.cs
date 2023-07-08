using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace UI.Main_Menu.Character_List_Menu
{
	public class StatsScrollMenuPanel : MonoBehaviour
	{
		[SerializeField] private GameObject statsMenuEntryPrefab;
		[SerializeField] private GameObject statsMenuEntryContainer;
		[SerializeField] private TextMeshProUGUI title;
		private List<GameObject> _statsMenuEntries;
		private int _currentEntryIndex;

		public void SetTitle(string titleText)
		{
			title.text = titleText;
		}
		
		public void AddEntry(string entryName, string value)
		{
			InstantiateList();
			var entry = _statsMenuEntries[_currentEntryIndex++];
			entry.GetComponent<StatsMenuEntry>().Setup(entryName, value);
			entry.SetActive(true);
		}
		
		public void ClearEntries()
		{
			InstantiateList();
			foreach (var entry in _statsMenuEntries)
			{
				entry.SetActive(false);
			}

			_currentEntryIndex = 0;
		}
		
		private void InstantiateList()
		{
			_statsMenuEntries ??= new List<GameObject>();
			if (_statsMenuEntries.Any())
				return;
			
			for (var i = 0; i < 50; i++)
			{
				var entry = Instantiate(statsMenuEntryPrefab, statsMenuEntryContainer.transform);
				entry.SetActive(false);
				_statsMenuEntries.Add(entry);
			}
		}
	}
}