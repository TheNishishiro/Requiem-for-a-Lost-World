using DefaultNamespace.Data;
using Events.Handlers;
using Events.Scripts;
using UnityEngine;

namespace Objects.Environment
{
    [RequireComponent(typeof(Light))]
    public class LightSettings : MonoBehaviour, ISettingsChangedHandler
    {
        private void Start()
        {
            ApplyLightSettings();
        }

        public void OnSettingsChanged()
        {
            ApplyLightSettings();
        }

        private void ApplyLightSettings()
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
        
        private void OnEnable()
        {
            SettingsChangedEvent.Register(this);
        }

        private void OnDisable()
        {
            SettingsChangedEvent.Unregister(this);
        }
    }
}