using System;
using Cinemachine;
using DefaultNamespace.Data;
using UnityEngine;

namespace Objects.Environment
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraSettings : MonoBehaviour
    {
        private void Start()
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
    }
}