using UnityEngine;

namespace UI.Shared
{
    public class ToggleElement : MonoBehaviour
    {
        public void Toggle()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}