using Unity.Netcode;
using UnityEngine;

namespace StarterAssets
{
    public class UICanvasControllerInput : NetworkBehaviour
    {
        [Header("Output")]
        private StarterAssetsInputs starterAssetsInputs;
        
        public void Start()
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }

        public override void OnDestroy()
        {
            if (NetworkManager.Singleton)
                NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            base.OnDestroy();
        }
		
        private void OnClientConnected(ulong clientId)
        {
            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                starterAssetsInputs = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponentInChildren<StarterAssetsInputs>();
            }
        }

        public void VirtualMoveInput(Vector2 virtualMoveDirection)
        {
            starterAssetsInputs.MoveInput(virtualMoveDirection);
        }

        public void VirtualLookInput(Vector2 virtualLookDirection)
        {
            starterAssetsInputs.LookInput(virtualLookDirection);
        }

        public void VirtualJumpInput(bool virtualJumpState)
        {
            starterAssetsInputs.JumpInput(virtualJumpState);
        }

        public void VirtualSprintInput(bool virtualSprintState)
        {
            starterAssetsInputs.SprintInput(virtualSprintState);
        }
        
    }

}
