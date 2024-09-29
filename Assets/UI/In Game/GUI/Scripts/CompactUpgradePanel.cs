using System;
using TMPro;
using UI.Labels.InGame.LevelUpScreen;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace UI.In_Game.GUI.Scripts
{
    public class CompactUpgradePanel : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TextMeshProUGUI upgradeText;
        [SerializeField] private Image background;
        [SerializeField] private UiItemContainer upgradeIcon;
        private float _timeVisible;
        
        public void Open(UpgradeEntry upgradeEntry)
        {
            gameObject.SetActive(true);
            upgradeIcon.Setup(upgradeEntry.GetUnlockIcon());
            upgradeText.text = $"{upgradeEntry.GetUnlockName()}" ;
            background.color = upgradeEntry.GetUpgradeColor();
            canvasGroup.alpha = 1;
            _timeVisible = 0f;
        }

        private void Update()
        {
            if (_timeVisible >= 5f)
            {
                gameObject.SetActive(false);
                return;
            }
            
            _timeVisible += Time.deltaTime;
            canvasGroup.alpha = math.lerp(1f, 0f, _timeVisible/5f);
        }
    }
}