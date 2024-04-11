using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Shared
{
    public class MaterialController : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField, Range(0, 10)] private float intensity = 1;
        [SerializeField] private Color color;

        private Material _materialInstance;

        private static readonly int Color1 = Shader.PropertyToID("_Color");

        public void Start()
        {
            _materialInstance = Instantiate(image.material);
            image.material = _materialInstance;
        }

        public void SetColor(Color newColor)
        {
            color = newColor;
            UpdateMaterialColor();
        }

        private void Update()
        {
            UpdateMaterialColor();
        }

        private void UpdateMaterialColor()
        {
            if (_materialInstance != null)
                _materialInstance.SetColor(Color1, color * intensity);
        }

        private void OnValidate()
        {
            UpdateMaterialColor();
        }
    }
}