using DefaultNamespace.Data;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class ServerEntry : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText;
        private ServerData _serverData;

        public void Setup(ServerData serverData)
        {
            _serverData = serverData;
            titleText.text = serverData.Name;
        }

        public void DeleteEntry()
        {
            ServerScreenManager.instance.DeleteServerEntry(_serverData, GetInstanceID());
        }

        public void EditEntry()
        {
            ServerScreenManager.instance.OpenEditEntry(_serverData, this);
        }

        public void Connect()
        {
            ServerScreenManager.instance.Connect(_serverData);
        }
    }
}