using System.Text;
using Objects.Characters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_Menu.Recollection_Menu
{
	public class CharacterDisplayPanel : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI characterName;
		[SerializeField] private TextMeshProUGUI characterStarRating;
		[SerializeField] private TextMeshProUGUI characterTitle;
		[SerializeField] private Image characterImage;
		
		public void Setup(CharacterData characterData)
		{
			characterName.text = characterData.Name;
			characterStarRating.text = new string('✦', characterData.StarRating);
			characterTitle.text = characterData.Title;
			characterImage.sprite = characterData.TransparentCard;
		}
	}
}