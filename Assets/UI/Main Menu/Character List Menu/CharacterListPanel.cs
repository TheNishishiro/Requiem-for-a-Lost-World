using DefaultNamespace.Data;
using Objects.Characters;
using TMPro;
using UnityEngine;
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
		private CharacterData _characterData;
		private SaveFile _saveFile;
		
		public CharacterListPanel Setup(CharacterData characterData)
		{
			_characterData = characterData;
			_saveFile = FindObjectOfType<SaveFile>();
			UpdateDisplayInfo();

			return this;
		}

		public void UpdateDisplayInfo()
		{
			var _characterSaveData = _saveFile.GetCharacterSaveData(_characterData.Id);
			
			nameTextField.text = _characterData.Name;
			levelTextField.text = $"lv. {_characterSaveData.Level}";
			experienceSlider.SetValue(_characterSaveData.Experience, _characterSaveData.ExperienceNeeded);
			lockPanel.SetActive(!_characterSaveData.IsUnlocked);
			characterPanel.sprite = _characterData.CharacterCard;
			experienceSliderImage.color = _characterData.ColorTheme;
		}

		public void OnClick()
		{
			var _characterSaveData = _saveFile.GetCharacterSaveData(_characterData.Id);
			if (!_characterSaveData.IsUnlocked)
				return;

			var characterListMenu = GetComponentInParent<CharacterListMenu>();
			characterListMenu.ActivateCharacter(_characterData);
		}
	}
}