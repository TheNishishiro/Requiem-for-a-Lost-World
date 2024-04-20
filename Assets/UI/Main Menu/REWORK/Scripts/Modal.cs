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
        [Space]
        [BoxGroup("Style")] [SerializeField] private Image imageLine1;
        [BoxGroup("Style")] [SerializeField] private Image imageLine2;
        [BoxGroup("Style")] [SerializeField] private Image imageLine3;
        [BoxGroup("Style")] [SerializeField] private Image imageLine4;

        private ModalResult _modalResult;

        public void Open(ButtonCombination buttonCombination, string title, string message, string textYes = "Yes", string textNo = "No", string textCancel = "Cancel", ModalState modalState = ModalState.None)
        {
            _modalResult = ModalResult.None;
            
            labelTitle.text = title;
            labelText.text = message;
            
            labelYes.text = textYes;
            labelCancel.text = textCancel;
            labelNo.text = textNo;
            
            buttonYes.gameObject.SetActive(buttonCombination is ButtonCombination.Yes or ButtonCombination.YesCancel or ButtonCombination.YesNoCancel);
            buttonCancel.gameObject.SetActive(buttonCombination is ButtonCombination.YesCancel or ButtonCombination.YesNoCancel);
            buttonNo.gameObject.SetActive(buttonCombination is ButtonCombination.YesNoCancel);
            gameObject.SetActive(true);
            
            imageLine1.color = imageLine2.color = imageLine3.color = imageLine4.color = 
                modalState switch
                {
                    ModalState.None => Color.white,
                    ModalState.Error => Color.red,
                    ModalState.Info => Color.cyan,
                    ModalState.Success => Color.green,
                    _ => throw new ArgumentOutOfRangeException(nameof(modalState), modalState, null)
                };
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public ModalResult GetResult()
        {
            return _modalResult;
        }

        public void SetResultState(int stateId)
        {
            _modalResult = (ModalResult)stateId;
            Close();
        }
    }
}