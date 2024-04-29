using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Data;
using Interfaces;
using Managers;
using NaughtyAttributes;
using Objects.Characters;
using Objects.Players.PermUpgrades;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class RuneScreenManager : MonoBehaviour, IStackableWindow
    {
        public static RuneScreenManager instance;
        [BoxGroup("Prefabs")] [SerializeField] private RuneEquipmentEntry runeEquipmentEntryPrefab;
        [Space]
        [BoxGroup("Containers")] [SerializeField] private Transform panelRunesEquipment;
        [BoxGroup("Containers")] [SerializeField] private GameObject panelDetails;
        [Space]
        [BoxGroup("Details")] [SerializeField] private TextMeshProUGUI textRuneDetails;
        [Space]
        [BoxGroup("Character")] [SerializeField] private TextMeshProUGUI textCharacterName;
        [BoxGroup("Character")] [SerializeField] private Image imageCharacterSignet;
        [Space]
        [BoxGroup("Containers")] [SerializeField] private List<RuneSlotEntry> runeSlotsOffensive;
        [BoxGroup("Containers")] [SerializeField] private List<RuneSlotEntry> runeSlotsDefensive;
        [BoxGroup("Containers")] [SerializeField] private List<RuneSlotEntry> runeSlotsUtility;
        [BoxGroup("Containers")] [SerializeField] private List<RuneSlotEntry> runeSlotsMixed;

        private CharacterSaveData _openedCharacterSaveData;
        private List<RuneEquipmentEntry> _runeEquipment;

        public void Start()
        {
            if (instance == null)
                instance = this;
        }

        private void Update()
        {
            if (!IsInFocus) return;
            
            if (Input.GetKeyDown(KeyCode.Escape))
                Close();
        }

        public void Open(CharacterData characterData, CharacterSaveData characterSaveData)
        {
            _openedCharacterSaveData = characterSaveData;
            textCharacterName.text = characterData.Name;
            imageCharacterSignet.sprite = characterData.signet;
            
            _runeEquipment ??= new List<RuneEquipmentEntry>();
            runeSlotsOffensive.ForEach(x => x.Clear());
            runeSlotsDefensive.ForEach(x => x.Clear());
            runeSlotsUtility.ForEach(x => x.Clear());
            runeSlotsMixed.ForEach(x => x.Clear());
            var characterRunes = characterSaveData.GetCharacterRunes();
            InsertRuneIntoSlot(StatCategory.Offensive, characterRunes);
            InsertRuneIntoSlot(StatCategory.Defensive, characterRunes);
            InsertRuneIntoSlot(StatCategory.Utility, characterRunes);
            
            foreach (var runeSaveData in SaveFile.Instance.Runes.OrderBy(x => x.statType.GetStatType())
                         .ThenByDescending(x => x.rarity)
                         .ThenBy(x => x.statType)
                         .ThenByDescending(x => x.runeValue))
            {
                CreateRuneEquipmentEntry(runeSaveData);
            }
            
            StackableWindowManager.instance.OpenWindow(this);
        }

        private void InsertRuneIntoSlot(StatCategory statCategory, IEnumerable<RuneSaveData> characterRunes, bool isInit = true)
        {
            var slots = statCategory switch
            {
                StatCategory.Offensive => runeSlotsOffensive,
                StatCategory.Defensive => runeSlotsDefensive,
                StatCategory.Utility => runeSlotsUtility,
                _ => throw new ArgumentOutOfRangeException(nameof(statCategory), statCategory, null)
            };
            
            foreach (var characterRune in characterRunes.Where(x => x.statType.GetStatType() == statCategory))
            {
                if (slots.Any(x => x.IsEmpty()))
                    slots.First(x => x.IsEmpty()).Setup(characterRune, isInit);
                else if (runeSlotsMixed.Any(x => x.IsEmpty()))
                    runeSlotsMixed.First(x => x.IsEmpty()).Setup(characterRune, isInit);
                else
                    return;
            }
        }

        private void CreateRuneEquipmentEntry(RuneSaveData runeSaveData)
        {
            var equipmentEntry = Instantiate(runeEquipmentEntryPrefab, panelRunesEquipment);
            equipmentEntry.Setup(runeSaveData);
            _runeEquipment.Add(equipmentEntry);
        }

        public void Close()
        {
            foreach (var runeEquipment in _runeEquipment)
            {
                Destroy(runeEquipment.gameObject);
            }
            _runeEquipment.Clear();
            SaveFile.Instance.Save();
            StackableWindowManager.instance.CloseWindow(this);
        }

        public void Equip(RuneSaveData runeSaveData, RuneEquipmentEntry runeEquipmentEntry)
        {
            if (!HasEmptySlots(runeSaveData.statType.GetStatType())) 
                return;
            
            _openedCharacterSaveData.EquipRune(runeSaveData);
            InsertRuneIntoSlot(runeSaveData.statType.GetStatType(), new[] { runeSaveData }, false);
            Discard(runeSaveData, runeEquipmentEntry);
        }

        private bool HasEmptySlots(StatCategory statCategory)
        {
            switch (statCategory)
            {
                case StatCategory.Offensive:
                    return runeSlotsOffensive.Any(x => x.IsEmpty()) || runeSlotsMixed.Any(x => x.IsEmpty());
                case StatCategory.Defensive:
                    return runeSlotsDefensive.Any(x => x.IsEmpty()) || runeSlotsMixed.Any(x => x.IsEmpty());
                case StatCategory.Utility:
                    return runeSlotsUtility.Any(x => x.IsEmpty()) || runeSlotsMixed.Any(x => x.IsEmpty());
                default:
                    throw new ArgumentOutOfRangeException(nameof(statCategory), statCategory, null);
            }
        }

        public void UnEquipRune(RuneSlotEntry runeSlotEntry, RuneSaveData runeSaveData)
        {
            _openedCharacterSaveData.UnEquipRune(runeSaveData);
            SaveFile.Instance.AddRune(runeSaveData);
            CreateRuneEquipmentEntry(runeSaveData);
            runeSlotEntry.Clear();
        }

        public void Discard(RuneSaveData runeSaveData, RuneEquipmentEntry runeEquipmentEntry)
        {
            SaveFile.Instance.DiscardRune(runeSaveData);
            _runeEquipment.Remove(runeEquipmentEntry);
            Destroy(runeEquipmentEntry.gameObject);
        }

        public void Filter(int categoryId)
        {
            _runeEquipment.ForEach(x => x.gameObject.SetActive(x.IsOfCategory((StatCategory)categoryId)));
        }

        public void RemoveFilter()
        {
            _runeEquipment.ForEach(x => x.gameObject.SetActive(true));
        }
        
        public bool IsInFocus { get; set; }
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}