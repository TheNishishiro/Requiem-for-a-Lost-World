using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Shared
{
    public class MaskedImage : MonoBehaviour
    {
        [SerializeField] private Image maskImage;
        [SerializeField] private Image image;
        [SerializeField] private bool visibleOnStart;

        public void Start()
        {
            image.enabled = visibleOnStart;
        }

        public void SetImage(Sprite sprite)
        {
            image.sprite = sprite;
        }

        public void SetVisible(bool visible)
        {
            image.enabled = visible;
        }
    }
}