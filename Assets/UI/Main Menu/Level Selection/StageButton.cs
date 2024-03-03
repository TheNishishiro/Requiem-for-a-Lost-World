using System;
using Managers;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Main_Menu.Level_Selection
{
	public class StageButton : MonoBehaviour
	{
		[SerializeField] private Button button;
		[SerializeField] private Image stageImage;
		[SerializeField] private TextMeshProUGUI stageTitle;
		[SerializeField] private Toggle toggle;
		public string sceneName;
		public string levelName;
		public Sprite levelImage;

		private void Awake()
		{
			button.onClick.AddListener(StartLevel);
			stageImage.sprite = levelImage;
			stageTitle.text = levelName;
		}

		private void StartLevel()
		{
			AudioManager.instance.PlayButtonConfirmClick();
			SceneManager.LoadScene($"Scenes/{sceneName}", LoadSceneMode.Single);
			SceneManager.LoadScene("Scenes/Essential", LoadSceneMode.Additive);
		}
	}
}