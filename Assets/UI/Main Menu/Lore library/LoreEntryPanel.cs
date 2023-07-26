using Objects.Characters;
using UI.Main_Menu.Story_Layout_Panel;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_Menu.Lore_library
{
	public class LoreEntryPanel : MonoBehaviour
	{
		[SerializeField] private LoreEntryContainer container;
		[SerializeField] private Image background;
		
		public void Open(LoreEntry loreEntry)
		{
			gameObject.SetActive(true);
			background.sprite = loreEntry.Background;
			if (loreEntry.Background == null)
				background.color = Color.black;
			container.Setup(loreEntry);
		}
		
		public void Close()
		{
			gameObject.SetActive(false);
			container.Clear();
		}
	}
}