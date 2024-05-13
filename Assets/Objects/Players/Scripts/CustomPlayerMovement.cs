using DefaultNamespace.Data;
using DefaultNamespace.Data.Settings;
using Managers;
using UI.In_Game.GUI.Scripts.Managers;
using UnityEngine;

namespace Objects.Players.Scripts
{
    public class CustomPlayerMovement
    {
        private static float _currentStaminaCooldown;
        private static float _currentStamina;
        private static float _staminaRegenCooldown = 2.0f;

        public static void Update()
        {
            RegenStamina();
            GuiManager.instance.UpdateStamina(_currentStamina, PlayerStatsScaler.GetScaler().GetStamina());
        }

        public static float GetSpeed()
        {
            var targetSpeed = PlayerStatsScaler.GetScaler().GetMovementSpeed();
            return Sprint(targetSpeed);
        }

        private static float Sprint(float inputSpeed)
        {
            if ((Input.GetKey(SaveFile.Instance.GetKeybinding(KeyAction.Sprint)) || GameManager.instance.IsPlayerSprinting) && _currentStamina > 0)
            {
                inputSpeed *= 2;
                _currentStamina -= Time.deltaTime;
                _currentStaminaCooldown = _staminaRegenCooldown;
            }

            return inputSpeed;
        }
        
        private static void RegenStamina()
        {
            if (Input.GetKey(SaveFile.Instance.GetKeybinding(KeyAction.Sprint)) || GameManager.instance.IsPlayerSprinting)
                return;

            if (_currentStaminaCooldown > 0)
            {
                _currentStaminaCooldown -= Time.deltaTime;
                return;
            }

            var stamina = PlayerStatsScaler.GetScaler().GetStamina();
            if (_currentStamina >= stamina)
                return;

            _currentStamina += Time.deltaTime;
        }
    }
}