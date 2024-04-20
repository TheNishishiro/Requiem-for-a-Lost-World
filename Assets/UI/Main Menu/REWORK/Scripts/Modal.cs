using System;
using DefaultNamespace.Data.Modals;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class Modal : MonoBehaviour
    {
        [BoxGroup("Buttons")] [SerializeField] private Button buttonYes;
        [BoxGroup("Buttons")] [SerializeField] private Button buttonNo;
        [BoxGroup("Buttons")] [SerializeField] private Button buttonCancel;
        [Space]
        [BoxGroup("Labels")] [SerializeField] private TextMeshProUGUI labelYes;
        [BoxGroup("Labels")] [SerializeField] private TextMeshProUGUI labelNo;
        [BoxGroup("Labels")] [SerializeField] private TextMeshProUGUI labelCancel;
        [Space]
        [BoxGroup("Modal")] [SerializeField] private TextMeshProUGUI labelTitle;
        [BoxGroup("Modal")] [SerializeField] private TextMeshProUGUI labelText;

        private ModalResult _modalResult;

        public void Open(ButtonCombination buttonCombination, string title, string message, string textYes = "Yes", string textNo = "No", string textCancel = "Cancel")
        {
            labelTitle.text = title;
            labelText.text = message;
            
            labelYes.text = textYes;
            labelCancel.text = textCancel;
            labelNo.text = textNo;
            
            buttonYes.gameObject.SetActive(true);
            buttonCancel.gameObject.SetActive(buttonCombination is ButtonCombination.YesCancel or ButtonCombination.YesNoCancel);
            buttonNo.gameObject.SetActive(buttonCombination is ButtonCombination.YesNoCancel);
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public ModalResult GetResult()
        {
            return _modalResult;
        }
    }
}