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
		
		private float originalY;
		private float randomFrequency;
		private float randomOffset;
		private float smoothFactor = 10;

		private void Start()
		{
			originalY = transform.position.y;

			randomFrequency = Random.Range(0.5f, 1.5f);
			randomOffset = Random.Range(0f, 2 * Mathf.PI);
		}

		private void FixedUpdate()
		{
			float yOffset = 10f * Mathf.Sin((Time.time * randomFrequency + randomOffset));
			Vector3 targetPosition = new Vector3(transform.position.x, originalY + yOffset, transform.position.z);
			transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothFactor);
		}

		public void Setup(CharacterData characterData)
		{
			characterName.text = characterData.Name;
			characterStarRating.text = new string('✦', characterData.StarRating);
			characterTitle.text = characterData.Title;
			characterImage.sprite = characterData.CharacterCard;
		}
	}
}