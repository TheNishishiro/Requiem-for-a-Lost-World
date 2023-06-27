using System.Collections;
using Objects.Characters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_Menu.Recollection_Menu
{
	public class GachaDisplayPanel : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI characterName;
		[SerializeField] private TextMeshProUGUI characterStarRating;
		[SerializeField] private Image characterImage;
		private Renderer _renderer;
		
		public void Setup(CharacterData characterData)
		{
			characterName.text = characterData.Name;
			characterImage.sprite = characterData.CharacterGachaArt;
			StartCoroutine(AnimateStars(characterData.StarRating));
		}

		private IEnumerator AnimateStars(int starRating)
		{
			for (var i = 0; i < starRating; i++)
			{
				characterStarRating.text += '✦';
				yield return new WaitForSeconds(0.1f);
			}
		}

		public void Clear()
		{
			characterName.text = "";
			characterStarRating.text = "";
			characterImage.sprite = null;
		}

	}
}