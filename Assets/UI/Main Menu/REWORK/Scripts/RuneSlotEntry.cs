using DefaultNamespace.Data;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class RuneSlotEntry : MonoBehaviour
    {
        [SerializeField] private GameObject container;
        [SerializeField] private SVGImage statTypeImage;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private TextMeshProUGUI textStats;
        private RuneSaveData _runeSaveData;
        
        public void Setup(RuneSaveData runeSaveData)
        {
            _runeSaveData = runeSaveData;
            textStats.text = runeSaveData.GetShortStats();
            container.SetActive(true);
        }

        public bool IsEmpty()
        {
            return _runeSaveData == null;
        }

        public void Clear()
        {
            _runeSaveData = null;
            container.SetActive(false);
        }

        public void UnEquipRune()
        {
            RuneScreenManager.instance.UnEquipRune(this, _runeSaveData);
        }
    }
}