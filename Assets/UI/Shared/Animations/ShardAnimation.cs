using System.Collections;
using System.Collections.Generic;
using Objects.Characters;
using UI.Main_Menu.REWORK.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Shared.Animations
{
    public class ShardAnimation : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        public float maxTiltAngle = 15f;
        public float idleMoveMagnitude = 10f;    // The magnitude of the idle movement
        public float idleMoveSpeed = 1f;         // The speed of the idle movement
        public float idleRotateSpeed = 30f;      // The speed of the idle rotation
        private Vector3 _idlePositionOffset;      // The offset for the idle position
        private Quaternion _idleRotationOffset; 
        private Vector3 _originalPosition;
        private Quaternion _originalRotation;
        
        private void Awake()
        {
            _originalPosition = rectTransform.anchoredPosition;
            _originalRotation = rectTransform.localRotation;
            idleMoveSpeed += GetInstanceID() % 10 / 10.0f;
            idleRotateSpeed += GetInstanceID() % 20 / 10.0f;
        }
    
        private void Update()
        {
            rectTransform.localRotation = _originalRotation * _idleRotationOffset;
            rectTransform.anchoredPosition3D = _originalPosition + _idlePositionOffset;
        
            _idlePositionOffset = new Vector3
            (
                Mathf.PingPong(Time.time * idleMoveSpeed, idleMoveMagnitude) - idleMoveMagnitude / 2,
                Mathf.PingPong(Time.time * idleMoveSpeed, idleMoveMagnitude) - idleMoveMagnitude / 2,
                0f
            );

            _idleRotationOffset = Quaternion.Euler(0f, 0f, Mathf.PingPong(Time.time * idleRotateSpeed, maxTiltAngle) - maxTiltAngle / 2);
        }
    }
}