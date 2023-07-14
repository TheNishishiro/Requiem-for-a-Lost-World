using System.Collections;
using UnityEngine;
using TMPro;

namespace UI.Shared.Animations
{
    public class TextGlowPulse : MonoBehaviour
    {
        public float PulseSpeed = 1f;
        public float EmissionIntensityMin = 0f;
        public float EmissionIntensityMax = 1f;
      
        private TextMeshProUGUI _tmpText;

        private void Awake()
        {
            _tmpText = GetComponent<TextMeshProUGUI>();
        }

        private void Update() 
        {
          float glow = EmissionIntensityMin + ((EmissionIntensityMax - EmissionIntensityMin) * (Mathf.Sin(Time.time * PulseSpeed) + 1f) / 2f);
          _tmpText.fontMaterial.SetFloat("_GlowPower", glow);
        }
    }
}
