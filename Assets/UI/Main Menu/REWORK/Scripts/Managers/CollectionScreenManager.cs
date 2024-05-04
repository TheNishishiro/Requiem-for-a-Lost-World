using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using DefaultNamespace.Data.Collection;
using Interfaces;
using Managers;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class CollectionScreenManager : MonoBehaviour, IStackableWindow
    {
        public static CollectionScreenManager instance;
        [BoxGroup("Prefabs")] [SerializeField] private CollectionEntry collectionEntryPrefab;
        [Space]
        [BoxGroup("Sources")] [SerializeField] private WeaponContainer containerWeapon;
        [BoxGroup("Sources")] [SerializeField] private ItemContainer containerItem;
        [Space]
        [BoxGroup("Section Containers")] [SerializeField] private Transform transformWeapon;
        [BoxGroup("Section Containers")] [SerializeField] private Transform transformItem;
        [BoxGroup("Section Containers")] [SerializeField] private Transform transformCharacter;
        [BoxGroup("Section Containers")] [SerializeField] private Transform transformShard;
        [Space]
        [BoxGroup("Section Labels")] [SerializeField] private GameObject labelWeapon;
        [BoxGroup("Section Labels")] [SerializeField] private GameObject labelItem;
        [BoxGroup("Section Labels")] [SerializeField] private GameObject labelCharacter;
        [BoxGroup("Section Labels")] [SerializeField] private GameObject labelShard;
        [Space]
        [BoxGroup("Buttons")] [SerializeField] private List<Button> buttonSections;
        [BoxGroup("Buttons")] [SerializeField] private List<Button> buttonStates;
        [Space]
        [BoxGroup("Button Section")] [SerializeField] private TextMeshProUGUI textButtonWeapons;
        [BoxGroup("Button Section")] [SerializeField] private TextMeshProUGUI textButtonItems;
        [BoxGroup("Button Section")] [SerializeField] private TextMeshProUGUI textButtonCharacters;
        [BoxGroup("Button Section")] [SerializeField] private TextMeshProUGUI textButtonShards;
        [Space]
        [BoxGroup("Styling")] [SerializeField] private Material materialSelectedText;
        [BoxGroup("Styling")] [SerializeField] private Material materialIdleText;
        [BoxGroup("Styling")] [SerializeField] private Color colorHighlight;
        [Space]
        [BoxGroup("Side Panel")] [SerializeField] private GameObject sidePanel;
        [BoxGroup("Side Panel")] [SerializeField] private TextMeshProUGUI labelTitle;
        [BoxGroup("Side Panel")] [SerializeField] private TextMeshProUGUI labelDescription;
        [BoxGroup("Side Panel")] [SerializeField] private TextMeshProUGUI labelUnlock;
        [BoxGroup("Side Panel")] [SerializeField] private TextMeshProUGUI labelUnlockDescription;
        
        private List<CollectionEntry> _collectionEntries;
        private int _currentSectionId;
        private int _currentStateId;
        private const float KeyHoldDelay = 0.25f;
        private float _keyNextActionTime = 0f;
        private bool _isSubsectionOpened;
        
        public void Start()
        {
            if (instance == null)
                instance = this;
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Close();
            
            if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow)) 
                _keyNextActionTime = 0;
            
            if (Input.GetKey(KeyCode.UpArrow) && Time.time > _keyNextActionTime)
            {
                _keyNextActionTime = Time.time + KeyHoldDelay;
                _currentSectionId--;
                if (_currentSectionId < 0) _currentSectionId = buttonSections.Count - 1;
                FilterSection(_currentSectionId);
            }
            else if (Input.GetKey(KeyCode.DownArrow) && Time.time > _keyNextActionTime)
            {
                _keyNextActionTime = Time.time + KeyHoldDelay;
                _currentSectionId++;
                if (_currentSectionId >= buttonSections.Count) _currentSectionId = 0;
                FilterSection(_currentSectionId);
            }                
            else if (Input.GetKey(KeyCode.LeftArrow) && Time.time > _keyNextActionTime)
            {
                _keyNextActionTime = Time.time + KeyHoldDelay;
                _currentStateId--;
                if (_currentStateId < 0) _currentStateId = buttonStates.Count - 1;
                FilterState(_currentStateId);
            }
            else if (Input.GetKey(KeyCode.RightArrow) && Time.time > _keyNextActionTime)
            {
                _keyNextActionTime = Time.time + KeyHoldDelay;
                _currentStateId++;
                if (_currentStateId >= buttonStates.Count) _currentStateId = 0;
                FilterState(_currentStateId);
            }    
        }

        public void FilterSection(int sectionId)
        {
            _currentSectionId = sectionId;
            var section = (CollectionSection)sectionId;
            labelWeapon.SetActive(section is CollectionSection.Weapon or CollectionSection.None);
            labelItem.SetActive(section is CollectionSection.Item or CollectionSection.None);
            labelCharacter.SetActive(section is CollectionSection.Character or CollectionSection.None);
            labelShard.SetActive(section is CollectionSection.Shard or CollectionSection.None);
            
            transformWeapon.gameObject.SetActive(section is CollectionSection.Weapon or CollectionSection.None);
            transformItem.gameObject.SetActive(section is CollectionSection.Item or CollectionSection.None);
            transformCharacter.gameObject.SetActive(section is CollectionSection.Character or CollectionSection.None);
            transformShard.gameObject.SetActive(section is CollectionSection.Shard or CollectionSection.None);
            
            buttonSections.ForEach(x => x.GetComponent<Image>().color = Color.clear);
            buttonSections.ForEach(x => x.GetComponentInChildren<TextMeshProUGUI>().fontSharedMaterial = materialIdleText);
            buttonSections[sectionId].GetComponent<Image>().color = colorHighlight;
            buttonSections[sectionId].GetComponentInChildren<TextMeshProUGUI>().fontSharedMaterial = materialSelectedText;
        }

        public void FilterState(int stateId)
        {
            _currentStateId = stateId;
            var state = (CollectionState)stateId;

            buttonStates.ForEach(x => x.GetComponentInChildren<TextMeshProUGUI>().fontSharedMaterial = materialIdleText);
            buttonStates[stateId].GetComponentInChildren<TextMeshProUGUI>().fontSharedMaterial = materialSelectedText;
            
            _collectionEntries.ForEach(x => x.VisibleState(state));
        }

        public void Display(string title, string description, string achievementRequirements, bool isUnlocked)
        {
            sidePanel.SetActive(true);
            labelTitle.text = isUnlocked ? title : "???";
            labelDescription.text = description;

            labelUnlock.gameObject.SetActive(!isUnlocked);
            labelUnlockDescription.gameObject.SetActive(!isUnlocked);
            if (!isUnlocked)
            {
                labelUnlockDescription.text = achievementRequirements;
            }
        }
        
        public void Open()
        {
            sidePanel.SetActive(false);
            _collectionEntries ??= new List<CollectionEntry>();
            if (_collectionEntries.Any())
            {
                _collectionEntries.ForEach(x => x.Refresh());
            }
            else
            {
                foreach (var weaponToggleableEntry in containerWeapon.GetWeapons())
                {
                    var collectionEntry = Instantiate(collectionEntryPrefab, transformWeapon);
                    collectionEntry.Setup(weaponToggleableEntry.weaponBase);
                    _collectionEntries.Add(collectionEntry);
                }
                foreach (var itemToggleableEntry in containerItem.GetItems())
                {
                    var collectionEntry = Instantiate(collectionEntryPrefab, transformItem);
                    collectionEntry.Setup(itemToggleableEntry.itemBase);
                    _collectionEntries.Add(collectionEntry);
                }
                foreach (var characterData in CharacterListManager.instance.GetCharacters().Where(x => x.IsPullable))
                {
                    var characterEntry = Instantiate(collectionEntryPrefab, transformCharacter);
                    characterEntry.Setup(characterData);
                    _collectionEntries.Add(characterEntry);
                    
                    foreach (var characterEidolon in characterData.Eidolons)
                    {
                        var collectionEntry = Instantiate(collectionEntryPrefab, transformShard);
                        collectionEntry.Setup(characterEidolon, characterData);
                        _collectionEntries.Add(collectionEntry);
                    }
                }
            }

            var pullableCharactersCount = CharacterListManager.instance.GetCharacters().Count(x => x.IsPullable);

            textButtonWeapons.text = $"Weapons ({containerWeapon.GetWeapons().Count(x => x.weaponBase.IsUnlocked(SaveFile.Instance))}/{containerWeapon.GetWeapons().Count})";
            textButtonItems.text = $"Items ({containerItem.GetItems().Count(x => x.itemBase.IsUnlocked(SaveFile.Instance))}/{containerItem.GetItems().Count})";
            textButtonCharacters.text = $"Characters ({SaveFile.Instance.CharacterSaveData.Count(x => x.Value.IsUnlocked)}/{pullableCharactersCount})";
            textButtonShards.text = $"Shards ({SaveFile.Instance.CharacterSaveData.Sum(x => x.Value.RankUpLevel)}/{pullableCharactersCount * 5})";
            
            FilterSection(0);
            FilterState(0);
            StackableWindowManager.instance.OpenWindow(this);
        }

        public void Close()
        {
            StackableWindowManager.instance.CloseWindow(this);
        }
        
        public bool IsInFocus { get; set; }
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}