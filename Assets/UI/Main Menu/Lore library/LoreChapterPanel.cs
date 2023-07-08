using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Main_Menu.Lore_library
{
	public class LoreChapterPanel : MonoBehaviour, IPointerClickHandler
	{
		[SerializeField] private TextMeshProUGUI title;
		private LoreChapterContainer _chapterContainer;
		
		public void SetTitle(string titleText)
		{
			title.text = titleText;
		}

		public void SetContainer(LoreChapterContainer chapterContainer)
		{
			_chapterContainer = chapterContainer;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			_chapterContainer.gameObject.SetActive(!_chapterContainer.gameObject.activeSelf);
		}
	}
}