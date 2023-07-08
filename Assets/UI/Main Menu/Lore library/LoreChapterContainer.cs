using Objects.Characters;
using UnityEngine;

namespace UI.Main_Menu.Lore_library
{
	public class LoreChapterContainer : MonoBehaviour
	{
		[SerializeField] private LoreChapterEntryPanel entryPanelPrefab;

		public void AddEntry(CharacterLoreEntry loreEntry)
		{
			var lorePanel = Instantiate(entryPanelPrefab, transform);
			lorePanel.Setup(loreEntry);
		}
	}
}