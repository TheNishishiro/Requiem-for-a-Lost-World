using System;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Settings;
using UI.In_Game.GUI.Scripts.Managers;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace.Components
{
    [RequireComponent(typeof(BoxCollider))]
    public class Interactable : MonoBehaviour
    {
        [SerializeField] private bool isOneTimeUse;
        [SerializeField] private bool destroyOnPickup;
        [SerializeField] private UnityEvent onInteract;
        [SerializeField] private string textPrompt = "interact";
        private Collider collider;
        private bool isPlayerInRange;

        private void Awake()
        {
            collider = GetComponent<Collider>();
        }

        private void Update()
        {
            if (collider.enabled && isPlayerInRange && Input.GetKeyDown(SaveFile.Instance.GetKeybinding(KeyAction.Interact)))
            {
                Interact();
            }
        }

        public void Interact()
        {
            onInteract.Invoke();
            if (isOneTimeUse || destroyOnPickup)
                GuiManager.instance.ToggleInteractionPrompt(false);
            
            if (isOneTimeUse)
                collider.enabled = false;
            if (destroyOnPickup)
                Destroy(gameObject);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            isPlayerInRange = true;
            GuiManager.instance.ToggleInteractionPrompt(true, textPrompt);
        }

        private void OnTriggerExit(Collider other)
        {
            isPlayerInRange = false;
            GuiManager.instance.ToggleInteractionPrompt(false);
        }
        
        
    }
}