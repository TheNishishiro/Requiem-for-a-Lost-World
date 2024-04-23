using TMPro;
using UI.Labels.InGame.LevelUpScreen;
using UnityEngine;
using UnityEngine.UI;

namespace UI.In_Game.GUI.Scripts
{
    public class UiUpgradeEntry : MonoBehaviour
    {
        [SerializeField] private Image iconUpgrade;
        [SerializeField] private Image backgroundRarity;
        [SerializeField] private ParticleSystem particlesRarity;
        [SerializeField] private Image backgroundType;
        [SerializeField] private TextMeshProUGUI textUpgradeName;
        [SerializeField] private TextMeshProUGUI textUpgradeDescription;
        private UpgradeEntry _upgradeEntry;

        public void Setup(UpgradeEntry upgradeEntry)
        {
            
        }
    }
}