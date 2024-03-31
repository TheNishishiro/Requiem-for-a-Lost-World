using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarouselUI
{
    public abstract class CarouselUI_Processor_Base : MonoBehaviour
    {
        [SerializeField, Tooltip("Carousel script that this processor tracks.")] private CarouselUIElement _associatedCarousel;

        protected int _storedSettingsIndex;

        private void Start()
        {
            //TRIES TO OBTAIN CAROUSEL SCRIPT
            if (TryGetComponent<CarouselUIElement>(out var element))
            { _associatedCarousel = element; }
            else
            { Debug.LogError($"Could not find CarouselUIElement on {this.gameObject.name}. Fix this."); }

            //SUBSCRIBE TO EVENTS
            _associatedCarousel.InputEvent += OnInputDetected;
        }

        private void OnEnable() //WHEN GAMEOBJECT IS ENABLED INDICES FOR CAROUSELS ARE IMMEDIATELY UPDATED!
        {
            UpdateCarouselUI();
        }

        private void OnInputDetected() //FIRED ON EVENT
        {
            DetermineOutput(_associatedCarousel.CurrentIndex);
        }

        /// <summary>
        /// Forces associated carousel element to update itself to the appropriate setting. When using inheritance, make sure base.UpdateCarouselUI() is placed AFTER custom code.
        /// </summary>
        protected virtual void UpdateCarouselUI()
        {
            //PROCESSING HAPPENS HERE

            if (_associatedCarousel.CurrentIndex != _storedSettingsIndex)
            {
                _associatedCarousel.UpdateIndex(_storedSettingsIndex);
            }
        }

        /// <summary>
        /// Determines result of carousel update. When using inheritance, there is no need to use base.DetermineOutput().
        /// </summary>
        /// <param name="input">Integer of setting.</param>
        protected virtual void DetermineOutput(int input)
        {
            print($"Input for {this.gameObject.name} is {input}");
        }

    }
}