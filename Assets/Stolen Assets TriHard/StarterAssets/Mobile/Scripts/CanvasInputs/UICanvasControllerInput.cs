using Unity.Netcode;
using UnityEngine;

namespace StarterAssets
{
    public class UICanvasControllerInput : NetworkBehaviour
    {
        [Header("Output")]
        private StarterAssetsInputs starterAssetsInputs;

        public void SetInputAsset(StarterAssetsInputs playerAssetsInputs)
        {
            starterAssetsInputs = playerAssetsInputs;
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
