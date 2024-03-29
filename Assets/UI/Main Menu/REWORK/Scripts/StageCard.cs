using Objects.Stage;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class StageCard : MonoBehaviour
    {
        [SerializeField] private Image imageDisplay;

        public void Setup(StageDefinition stageDefinition)
        {
            imageDisplay.sprite = stageDefinition.background;
        }
    }
}