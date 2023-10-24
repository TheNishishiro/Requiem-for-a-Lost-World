using DefaultNamespace.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.Main_Menu.Recollection_Menu
{
    public class PityTextLabel : MonoBehaviour
    {
        private SaveFile _saveFile;
        [SerializeField] private TextMeshProUGUI pityText;

        private void Awake()
        {
            _saveFile = FindObjectOfType<SaveFile>();
        }

        public void Update()
        {
            if (Time.frameCount % 10 != 0)
                return;
			
            if (_saveFile.Pity < 10 )
                pityText.text = $"Until new: {10 - _saveFile.Pity}";
            else
                pityText.text = $"New guranteed";
        }
    }
}