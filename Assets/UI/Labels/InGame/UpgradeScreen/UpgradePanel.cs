using Managers;
using TMPro;
using UI.Labels.InGame.LevelUpScreen;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace UI.Labels.InGame.UpgradeScreen
{
	public class UpgradePanel : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI labelName;
		[SerializeField] private Image upgradeIcon;
		[SerializeField] private ParticleSystem highlightParticles;
		private UpgradeEntry _upgradeEntry;
		
		public void SetUpgradeData(UpgradeEntry upgradeEntry)
		{
			_upgradeEntry = upgradeEntry;
			labelName.text = upgradeEntry.GetUnlockName();
			upgradeIcon.sprite = upgradeEntry.GetUnlockIcon();
			var p = highlightParticles.main;
			p.startColor = upgradeEntry.GetUpgradeColor();
		}

		public void Open()
		{
			ChestPanelManager.instance.ShowDescription(_upgradeEntry.GetUnlockDescription());
		}
	}
}