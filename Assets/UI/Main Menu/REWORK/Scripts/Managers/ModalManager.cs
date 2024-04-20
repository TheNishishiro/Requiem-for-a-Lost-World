using DefaultNamespace.Data.Modals;
using UnityEngine;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class ModalManager : MonoBehaviour
    {
        public static ModalManager instance;
        [SerializeField] private Modal modal;
        
        public void Start()
        {
            if (instance == null)
                instance = this;
        }
        
        public void Open(ButtonCombination buttonCombination, string title, string message, string textYes = "Yes", string textNo = "No", string textCancel = "Cancel")
        {
            modal.Open(buttonCombination, title, message, textYes, textNo, textCancel);
        }

        public void Close()
        {
            modal.Close();
        }

        public ModalResult GetResult()
        {
            return modal.GetResult();
        }
    }
}