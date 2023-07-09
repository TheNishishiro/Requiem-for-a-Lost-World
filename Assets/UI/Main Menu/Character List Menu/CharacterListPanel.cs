using System;
using System.Collections;
using DefaultNamespace.Data;
using Managers;
using Objects.Characters;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Main_Menu.Character_List_Menu
{
	public class CharacterListPanel : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI nameTextField;
		[SerializeField] private TextMeshProUGUI levelTextField;
		[SerializeField] private CharacterExpBar experienceSlider;
		[SerializeField] private Image experienceSliderImage;
		[SerializeField] private GameObject lockPanel;
		[SerializeField] private Image characterPanel;
		[HideInInspector] public CharacterData characterData;
		private SaveFile _saveFile;
		private CharacterInfoPanel _characterInfoPanel;
		private ScrollRect _scrollRect;
		
		public void Setup(CharacterData characterData, CharacterInfoPanel characterInfoPanel, ScrollRect scrollRect)
		{
			_scrollRect = scrollRect;
			this.characterData = characterData;
			_characterInfoPanel = characterInfoPanel;
			_saveFile = FindObjectOfType<SaveFile>();
			UpdateDisplayInfo();
		}

		public void UpdateDisplayInfo()
		{
			var _characterSaveData = _saveFile.GetCharacterSaveData(characterData.Id);
			
			nameTextField.text = characterData.Name;
			levelTextField.text = $"lv. {_characterSaveData.Level}";
			experienceSlider.SetValue(_characterSaveData.Experience, _characterSaveData.ExperienceNeeded);
			lockPanel.SetActive(!_characterSaveData.IsUnlocked);
			characterPanel.sprite = characterData.CharacterCard;
			experienceSliderImage.color = characterData.ColorTheme;
		}

		public void OnClick()
		{
			var _characterSaveData = _saveFile.GetCharacterSaveData(characterData.Id);
			if (!_characterSaveData.IsUnlocked)
				return;

			foreach (var character in CharacterListManager.instance.GetCharacters())
			{
				character.Deactivate();
			}
			
			characterData.Activate();
			_characterInfoPanel.gameObject.SetActive(!_characterInfoPanel.gameObject.activeSelf);
			_characterInfoPanel.SetCharacterData(characterData, _saveFile.GetCharacterSaveData(characterData.Id));

			foreach (var characterInfoPanel in FindObjectsOfType<CharacterInfoPanel>())
			{
				if (characterInfoPanel == _characterInfoPanel)
					continue;
				characterInfoPanel.gameObject.SetActive(false);
			}
			
			if (_characterInfoPanel.gameObject.activeSelf)
				CenterOnItem(GetComponent<RectTransform>());

			return;
		}
		
		public void CenterOnItem(RectTransform target)
		{
			StartCoroutine(CenterOnItemCoroutine(target));
		}

		private IEnumerator CenterOnItemCoroutine(RectTransform target)
		{
			Canvas.ForceUpdateCanvases();

			var newPosition = (Vector2)_scrollRect.transform.InverseTransformPoint(_scrollRect.content.position) - (Vector2)_scrollRect.transform.InverseTransformPoint(target.position);
			var relativePosition = new Vector2(newPosition.x, _scrollRect.content.anchoredPosition.y);
			var currentPosition = _scrollRect.content.anchoredPosition;
			
			var time = 0f;
			var duration = 0.2f;

			while (time < duration)
			{
				time += Time.deltaTime;
				_scrollRect.content.anchoredPosition = Vector2.Lerp(currentPosition, relativePosition, time / duration);
				yield return null;
			}
		}
	}
}