using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using NaughtyAttributes;
using Objects.Stage;
using TMPro;
using UI.Labels.InGame;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace UI.In_Game.GUI.Scripts.Managers
{
    public class GuiManager : MonoBehaviour
    {
        public static GuiManager instance;
        
        [Space]
        [BoxGroup("Player Stats")] [SerializeField] private TextMeshProUGUI labelCharacterName;
        [BoxGroup("Player Stats")] [SerializeField] private TextMeshProUGUI labelLevel;
        [BoxGroup("Player Stats")] [SerializeField] private Image imageAvatar;
        [Space]
        [BoxGroup("Player Stats Bars")] [SerializeField] private Slider sliderHealth;
        [BoxGroup("Player Stats Bars")] [SerializeField] private Slider sliderExperience;
        [Space]
        [BoxGroup("Player Stats Color Theme")] [SerializeField] private DiamondGraph imageAvatarBorder;
        [BoxGroup("Player Stats Color Theme")] [SerializeField] private DiamondGraph imageSmallTriangle;
        [BoxGroup("Player Stats Color Theme")] [SerializeField] private DiamondGraph imageBigTriangle;
        [BoxGroup("Player Stats Color Theme")] [SerializeField] private Image imageNameBackground;
        [BoxGroup("Player Stats Color Theme")] [SerializeField] private UILineRenderer lineRendererNameBorder;
        [BoxGroup("Player Stats Color Theme")] [SerializeField] private UICircle circleNameDetail;
        [BoxGroup("Player Stats Color Theme")] [SerializeField] private UILineRenderer lineRendererHpLineSemiTransparent;
        [BoxGroup("Player Stats Color Theme")] [SerializeField] private UILineRenderer lineRendererHpLine;
        [BoxGroup("Player Stats Color Theme")] [SerializeField] private UICircle circleHpFull;
        [BoxGroup("Player Stats Color Theme")] [SerializeField] private UICircle circleHpEmpty;
        [BoxGroup("Player Stats Color Theme")] [SerializeField] private Image imageHpDetailLeft;
        [BoxGroup("Player Stats Color Theme")] [SerializeField] private Image imageHpDetailRight;
        [BoxGroup("Player Stats Color Theme")] [SerializeField] private UILineRenderer lineRendererExpLine;
        [BoxGroup("Player Stats Color Theme")] [SerializeField] private UICircle circleExpFull;
        [BoxGroup("Player Stats Color Theme")] [SerializeField] private Image imageExpDetailLeft;
        [Space]
        [BoxGroup("Movement Container")] [SerializeField] private Slider sliderStamina;
        [BoxGroup("Movement Container")] [SerializeField] private List<GameObject> dashIndicators;
        [Space]
        [BoxGroup("Item Container Color Theme")] [SerializeField] private UILineRenderer lineRendererWeapons;
        [BoxGroup("Item Container Color Theme")] [SerializeField] private UILineRenderer lineRendererItems;
        [Space]
        [BoxGroup("Skill")] [SerializeField] private TextMeshProUGUI textSkillName;
        [BoxGroup("Skill")] [SerializeField] private Image imageSkillIcon;
        [BoxGroup("Skill")] [SerializeField] private Image imageSkillCooldown;
        [Space]
        [BoxGroup("Skill Color Theme")] [SerializeField] private DiamondGraph imageSkillBigTriangle;
        [BoxGroup("Skill Color Theme")] [SerializeField] private DiamondGraph imageSkillBorder;
        [BoxGroup("Skill Color Theme")] [SerializeField] private Image imageSkillNameBackground;
        [BoxGroup("Skill Color Theme")] [SerializeField] private UILineRenderer imageSkillNameBorder;
        [BoxGroup("Skill Color Theme")] [SerializeField] private UICircle imageSkillNameCircle;
        [BoxGroup("Skill Color Theme")] [SerializeField] private ParticleSystem particleSkillActiveRipples;
        [BoxGroup("Skill Color Theme")] [SerializeField] private ParticleSystem particleSkillActive;
        [Space]
        [BoxGroup("Game Stats")] [SerializeField] private UiInfoEntry infoEntryGold;
        [BoxGroup("Game Stats")] [SerializeField] private UiInfoEntry infoEntryGem;
        [BoxGroup("Game Stats")] [SerializeField] private UiInfoEntry infoEntryKills;
        [Space]
        [BoxGroup("Item Container")] [SerializeField] private List<UiItemContainer> weapons;
        [BoxGroup("Item Container")] [SerializeField] private List<UiItemContainer> items;
        
        public void Awake()
        {
            if (instance == null)
                instance = this;
            
            var characterData = GameData.GetPlayerCharacterData();
            labelCharacterName.text = characterData.Name;
            imageAvatar.sprite = characterData.Avatar;
            imageSkillIcon.sprite = characterData.AbilityIcon;
            textSkillName.text = characterData.AbilityName;

            imageAvatarBorder.color = characterData.ColorTheme;
            imageSmallTriangle.color = characterData.ColorTheme;
            imageBigTriangle.color = new Color(characterData.ColorTheme.r, characterData.ColorTheme.g, characterData.ColorTheme.b, 200f/255f);
            imageNameBackground.color = characterData.ColorTheme;
            lineRendererNameBorder.color = characterData.ColorTheme;
            circleNameDetail.color = characterData.ColorTheme;
            lineRendererHpLineSemiTransparent.color = new Color(characterData.ColorTheme.r, characterData.ColorTheme.g, characterData.ColorTheme.b, 120f/255f);
            lineRendererHpLine.color = characterData.ColorTheme;
            circleHpFull.color = characterData.ColorTheme;
            circleHpEmpty.color = characterData.ColorTheme;
            imageHpDetailLeft.color = characterData.ColorTheme;
            imageHpDetailRight.color = characterData.ColorTheme;
            lineRendererExpLine.color = characterData.ColorTheme;
            circleExpFull.color = characterData.ColorTheme;
            imageExpDetailLeft.color = characterData.ColorTheme;
            lineRendererWeapons.color = characterData.ColorTheme;
            lineRendererItems.color = characterData.ColorTheme;
            imageSkillBigTriangle.color = new Color(characterData.ColorTheme.r, characterData.ColorTheme.g, characterData.ColorTheme.b, 200f/255f);
            imageSkillBorder.color = characterData.ColorTheme;
            imageSkillNameBackground.color = characterData.ColorTheme;
            imageSkillNameBorder.color = characterData.ColorTheme;
            imageSkillNameCircle.color = characterData.ColorTheme;
            var p1 = particleSkillActiveRipples.main;
            p1.startColor = characterData.ColorTheme;
            var p2 = particleSkillActiveRipples.main;
            p2.startColor = new ParticleSystem.MinMaxGradient(characterData.ColorTheme, Color.white);

            infoEntryGold.SetTheme(characterData.ColorTheme);
            infoEntryGem.SetTheme(characterData.ColorTheme);
            infoEntryKills.SetTheme(characterData.ColorTheme);
            
            SetLevelText(1);
            weapons.ForEach(x => x.gameObject.SetActive(false));
            items.ForEach(x => x.gameObject.SetActive(false));
        }

        private void Update()
        {
            if (Time.frameCount % 60 == 0)
            {
                infoEntryGold.SetText(GameResultData.Gold);
                infoEntryGem.SetText(GameResultData.Gems);
                infoEntryKills.SetText(GameResultData.MonstersKilled);
            }
        }

        public void UpdateItems()
        {
            weapons.ForEach(x => x.gameObject.SetActive(false));
            items.ForEach(x => x.gameObject.SetActive(false));

            var index = 0;
            foreach (var weapon in WeaponManager.instance.GetUnlockedWeaponsAsInterface())
            {
                weapons[index++].Setup(weapon);
            }

            index = 0;
            foreach (var item in WeaponManager.instance.GetUnlockedItemsAsInterface())
            {
                items[index++].Setup(item);
            }
        }
        
        public void SetLevelText(int level)
        {
            labelLevel.text = $"lv. {level}";
        }

        public void UpdateHealth(float value, float maxValue)
        {
            UpdateSlider(sliderHealth, value, maxValue);
        }

        public void UpdateExperience(float value, float maxValue)
        {
            UpdateSlider(sliderExperience, value, maxValue);
        }

        public void UpdateStamina(float value, float maxValue)
        {
            UpdateSlider(sliderStamina, value, maxValue);
        }

        public void UpdateDashes(int dashCount)
        {
            var index = 0;
            dashIndicators.ForEach(x =>
            {
                x.SetActive(index++ < dashCount);
            });
        }

        public void UpdateAbilityCooldown(float currentSkillCooldown, float skillCooldown)
        {
            imageSkillCooldown.fillAmount = currentSkillCooldown / skillCooldown;
        }

        private void UpdateSlider(Slider slider, float value, float maxValue)
        {
            slider.value = value;
            slider.maxValue = maxValue;
        }
    }
}