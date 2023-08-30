using System;
using System.Collections.Generic;
using DefaultNamespace.Data;
using Interfaces;
using UnityEngine;

namespace UI.Main_Menu.Unlockables_Menu
{
	public class UnlockableMenu : MonoBehaviour
	{
		[SerializeField] private GameObject unlockableEntryPrefab;
		[SerializeField] private Transform container;
		[SerializeField] private WeaponContainer weaponContainer;
		[SerializeField] private ItemContainer itemContainer;
		private List<UnlockableEntry> unlockableEntries = new List<UnlockableEntry>();

		private void Start()
		{
			GenerateUnlockableEntries();
		}

		private void GenerateUnlockableEntries()
		{
			var saveFile = FindObjectOfType<SaveFile>();
			var weapons = weaponContainer.GetWeapons();
			var items = itemContainer.GetItems();

			foreach (var weapon in weapons)
			{
				CreateUnlockableEntry(weapon.weaponBase, saveFile);
			}
			foreach (var item in items)
			{
				CreateUnlockableEntry(item.itemBase, saveFile);
			}
		}
		
		private void CreateUnlockableEntry(IPlayerItem item, SaveFile saveFile)
		{
			var go = Instantiate(unlockableEntryPrefab, container);
			var unlockableEntry = go.GetComponent<UnlockableEntry>();
			unlockableEntry.SetItem(item, saveFile);
			unlockableEntries.Add(unlockableEntry);
		}

		private void OnEnable()
		{
			UpdateLocks();
		}
		
		private void UpdateLocks()
		{
			unlockableEntries ??= new List<UnlockableEntry>();
			foreach (var unlockableEntry in unlockableEntries)
			{
				unlockableEntry.UpdateLock();
			}
		}
	}
}