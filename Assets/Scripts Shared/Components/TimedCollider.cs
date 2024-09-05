using System;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace.Components
{
    [RequireComponent(typeof(BoxCollider))]
    public class TimedCollider : MonoBehaviour
    {
        [SerializeField] private float timeRequired;
        [SerializeField] private UnityEvent onTimerElapsed;
        private float _timeInside;
        private bool _hasStarted;

        private void Update()
        {
            if (_hasStarted)
                _timeInside += Time.deltaTime;
            if (_timeInside > timeRequired)
                onTimerElapsed.Invoke();
        }

        private void OnTriggerStay(Collider other)
        {
            _hasStarted = true;
        }

        private void OnTriggerExit(Collider other)
        {
            _hasStarted = false;
            _timeInside = 0;
        }
    }
}