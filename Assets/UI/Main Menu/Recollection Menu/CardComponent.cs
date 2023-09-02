using System;
using System.Collections;
using Objects.Characters;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Main_Menu.Recollection_Menu
{
	public class CardComponent : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
	{
		[SerializeField] private TextMeshProUGUI unlockText;
		[SerializeField] private TextMeshProUGUI characterNameText;
		[SerializeField] private TextMeshProUGUI characterStarRatingText;
		[SerializeField] private GameObject collectButton;
		[SerializeField] private GameObject dragIcon;
		private CharacterData _characterData;
		private float rotationY = 0f;  
		private bool isRotating = false;    
		private float targetRotationY;
		private const float RotationSpeed = 120f;
		private const float Sensitivity = 0.5f;
		
		private void OnEnable()
		{
			GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, 0);
			rotationY = 0;
			isRotating = false;
			dragIcon.SetActive(true);
		}

		private void OnDisable()
		{
			SetElementsActive(false);
		}

		public void SetCharacter(CharacterData characterData)
		{
			_characterData = characterData;
			characterNameText.text = characterData.Name;
			GetComponent<Image>().sprite = characterData.CharacterCard;
		}

		private IEnumerator AnimateStars(int starRating)
		{
			characterStarRatingText.text = string.Empty;
			for (var i = 0; i < starRating; i++)
			{
				characterStarRatingText.text += '✦';
				yield return new WaitForSeconds(0.1f);
			}
		}
		
		public void OnDrag(PointerEventData eventData)
		{
			// Get change in the mouse position
			var deltaX = eventData.delta.x;
			// Calculate new rotation around Y-axis
			rotationY -= deltaX * Sensitivity;
			// Clamp the rotation angle to within -180 ~ 180 degrees
			rotationY = Mathf.Clamp(rotationY, -180, 180f);
			// Apply the rotation
			GetComponent<RectTransform>().localEulerAngles = new Vector3(0, rotationY, 0);
			
			dragIcon.SetActive(Math.Abs(rotationY) < 30);
		}

		public void OnPointerDown(PointerEventData eventData)
		{
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			isRotating = true;
			if(Mathf.Abs(rotationY) >= 90)
			{
				targetRotationY = rotationY >= 0 ? 180 : -180;
			}
			else
			{
				targetRotationY = 0;
			}
		}
		
		void Update()
		{
			if(isRotating)
			{
				float newRotationY = Mathf.MoveTowardsAngle(GetComponent<RectTransform>().localEulerAngles.y, targetRotationY, RotationSpeed * Time.deltaTime);

				GetComponent<RectTransform>().localEulerAngles = new Vector3(0, newRotationY, 0);

				if(Mathf.Abs(newRotationY - targetRotationY) < 0.01f)
				{
					isRotating = false;
					rotationY = targetRotationY;

					SetElementsActive(targetRotationY != 0);
				}
			}
		}
		
		private void SetElementsActive(bool active)
		{
			unlockText.gameObject.SetActive(active);
			collectButton.SetActive(active);

			if (active)
			{
				StartCoroutine(AnimateStars(_characterData.StarRating));
			}
		}

		public void SetColor(Color pullColor)
		{
			var image = GetComponent<Image>();
			if (image == null) return;
			
			var currentMaterial = image.material;
			var factor = Mathf.Pow(2,3.5f);
			Color hdrColor = new (pullColor.r*factor,pullColor.g*factor,pullColor.b*factor);
			currentMaterial.SetColor("_RarityColor", hdrColor);
			image.material = currentMaterial;
		}
	}  
}