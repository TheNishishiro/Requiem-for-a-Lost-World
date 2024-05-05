using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Collection;
using DefaultNamespace.Data.Tutorials;
using Interfaces;
using Managers;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class TutorialScreenManager : MonoBehaviour, IStackableWindow
    {
        public static TutorialScreenManager instance;
        [BoxGroup("Section Labels")] [SerializeField] private GameObject containerWelcome;
        [BoxGroup("Section Labels")] [SerializeField] private GameObject containerRarities;
        [BoxGroup("Section Labels")] [SerializeField] private GameObject containerElementalReactions;
        [BoxGroup("Section Labels")] [SerializeField] private GameObject containerCoop;
        [Space]
        [BoxGroup("Buttons")] [SerializeField] private List<Button> buttonSections;
        [Space]
        [BoxGroup("Styling")] [SerializeField] private Material materialSelectedText;
        [BoxGroup("Styling")] [SerializeField] private Material materialIdleText;
        [BoxGroup("Styling")] [SerializeField] private Color colorHighlight;
        
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
        }

        public void FilterSection(int sectionId)
        {
            _currentSectionId = sectionId;
            var section = (TutorialSection)sectionId;
            containerWelcome.SetActive(section is TutorialSection.Welcome);
            containerRarities.SetActive(section is TutorialSection.Rarities);
            containerElementalReactions.SetActive(section is TutorialSection.Reactions);
            containerCoop.SetActive(section is TutorialSection.Coop);
            
            buttonSections.ForEach(x => x.GetComponent<Image>().color = Color.clear);
            buttonSections.ForEach(x => x.GetComponentInChildren<TextMeshProUGUI>().fontSharedMaterial = materialIdleText);
            buttonSections[sectionId].GetComponent<Image>().color = colorHighlight;
            buttonSections[sectionId].GetComponentInChildren<TextMeshProUGUI>().fontSharedMaterial = materialSelectedText;
        }
        
        public void Open()
        {
            FilterSection(0);
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