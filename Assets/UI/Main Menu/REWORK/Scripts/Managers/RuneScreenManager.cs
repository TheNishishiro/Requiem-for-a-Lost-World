using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Data;
using Interfaces;
using Managers;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

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
        [BoxGroup("Containers")] [SerializeField] private List<RuneSlotEntry> runeSlots;

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

        public void Open(CharacterSaveData characterSaveData)
        {
            _openedCharacterSaveData = characterSaveData;
            _runeEquipment ??= new List<RuneEquipmentEntry>();
            runeSlots.ForEach(x => x.Clear());
            foreach (var characterRune in characterSaveData.GetCharacterRunes().TakeWhile(_ => runeSlots.Any(x => x.IsEmpty())))
            {
                runeSlots.First(x => x.IsEmpty()).Setup(characterRune);
            }
            
            foreach (var runeSaveData in SaveFile.Instance.Runes)
            {
                CreateRuneEquipmentEntry(runeSaveData);
            }
            
            StackableWindowManager.instance.OpenWindow(this);
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
            if (!runeSlots.Any(x => x.IsEmpty())) 
                return;
            
            _openedCharacterSaveData.EquipRune(runeSaveData);
            runeSlots.First(x => x.IsEmpty()).Setup(runeSaveData);
            
            Discard(runeSaveData, runeEquipmentEntry);
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

        public void DisplayDetails(RuneSaveData runeSaveData)
        {
            textRuneDetails.text = runeSaveData?.GetName();
            panelDetails.SetActive(runeSaveData != null);
        }

        public void HideDetails()
        {
            panelDetails.SetActive(false);
        }
        
        public bool IsInFocus { get; set; }
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}