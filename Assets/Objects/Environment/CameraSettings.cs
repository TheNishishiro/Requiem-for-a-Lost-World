using System;
using Cinemachine;
using DefaultNamespace.Data;
using Events.Handlers;
using Events.Scripts;
using UnityEngine;

namespace Objects.Environment
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraSettings : MonoBehaviour, ISettingsChangedHandler
    {
        private void Start()
        {
            ApplyCameraSettings();
        }

        public void OnSettingsChanged()
        {
            ApplyCameraSettings();
        }

        private void ApplyCameraSettings()
        {
            var saveFile = FindFirstObjectByType<SaveFile>();
            if (saveFile == null)
                Debug.Log("Save file not found");
            
            var cameraComponent = GetComponent<CinemachineVirtualCamera>();
            cameraComponent.m_Lens.FarClipPlane = saveFile.ConfigurationFile.RenderDistance switch
            {
                0 => 35,
                1 => 70,
                2 => 120,
                3 => 500
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