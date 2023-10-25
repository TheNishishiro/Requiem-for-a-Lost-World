using DefaultNamespace.Data;
using UnityEngine;

namespace Objects.Environment
{
    [RequireComponent(typeof(Light))]
    public class LightSettings : MonoBehaviour
    {
        private void Start()
        {
            var saveFile = FindObjectOfType<SaveFile>();
            if (saveFile == null)
            {
                Debug.Log("Save file not found");
                return;
            }

            var lightComponent = GetComponent<Light>();
            lightComponent.shadows = QualitySettings.shadows switch
            {
                ShadowQuality.Disable => LightShadows.None,
                ShadowQuality.HardOnly => LightShadows.Hard,
                _ => lightComponent.shadows
            };
        }
    }
}