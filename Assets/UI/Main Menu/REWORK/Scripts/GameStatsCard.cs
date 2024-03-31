using TMPro;
using UnityEngine;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class GameStatsCard : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI labelValue;

        public void Set(string value)
        {
            labelValue.text = value;
        }
        
        public void Set(int value)
        {
            labelValue.text = value.ToString();
        }
        
        public void Set(float value)
        {
            labelValue.text = value.ToString("F");
        }
        
        public void SetPercent(float value)
        {
            labelValue.text = $"{value * 100:F}%";
        }
    }
}