using System.Collections.Generic;
using System.Linq;
using Interfaces;
using TMPro;
using UI.In_Game.GUI.Scripts;
using UI.Shared;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Labels.InGame.MP_List
{
    public class MpCharacterInfoEntry : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nicknameText;
        [SerializeField] private ExperienceBar healthBar;
        [SerializeField] private Image avatar;
        [SerializeField] private List<UiItemContainer> weaponIcons;
        [SerializeField] private List<UiItemContainer> itemIcons;
        private readonly Dictionary<string, UiItemContainer> _containerCache = new ();
        private readonly Dictionary<string, int> _levelCache = new ();

        public void SetData(float currentHp, float maxHp, int level, string playerName)
        {
            nicknameText.text = playerName;
            healthBar.UpdateSlider(currentHp, maxHp);
            healthBar.SetLevelText(level);
        }

        public void SetAvatar(Sprite characterAvatar)
        {
            avatar.sprite = characterAvatar;
        }

        public void AddOrUpdateItem(IPlayerItem playerItem, bool isItem, bool isWeapon)
        {
            if (playerItem == null)
                return;

            if (_containerCache.ContainsKey(playerItem.NameField))
            {
                _levelCache[playerItem.NameField]++;
                _containerCache[playerItem.NameField].Setup(playerItem.IconField, _levelCache[playerItem.NameField]);
            }
            else
            {
                UiItemContainer container;
                if (isWeapon)
                {
                    if (!weaponIcons.Any()) return;
                    
                    container = weaponIcons.First();
                    weaponIcons.Remove(container);
                }
                else if (isItem)
                {
                    if (!itemIcons.Any()) return;
                    
                    container = itemIcons.First();
                    itemIcons.Remove(container);
                }
                else
                {
                    return;
                }

                _levelCache.Add(playerItem.NameField, 1);
                _containerCache.Add(playerItem.NameField, container);
                container.Setup(playerItem.IconField, 1);
            }
        }
    }
}