using System;
using System.Collections;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Settings;
using Managers;
using Objects.Stage;
using UI.In_Game.GUI.Scripts.Managers;
using UnityEngine;

namespace Objects.Players.Scripts
{
    public class PlayerDashComponent : MonoBehaviour
    {
        private CharacterController _characterController;
        private const float DashCooldown = 10f;
        public float dashSpeed = 20f;
        public float dashDuration = 0.2f;
        private float _dashTimeLeft;
        private bool _isDashing;
        private float _currentDashCooldown = 0f;
        private int _dashStacks = 2;
        private Transform _transform;

        private int DashStacks
        {
            get => _dashStacks;
            set
            {
                _dashStacks = value;
                GuiManager.instance.UpdateDashes(_dashStacks);
            }
        }
    
        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
			DashStacks = PlayerStatsScaler.GetScaler().GetDashCount();
        }

        private void Update()
        {
            _transform = GameManager.instance.PlayerTransform;
            if (_transform == null) return;
            
            if (_dashStacks < PlayerStatsScaler.GetScaler().GetDashCount())
            {
                _currentDashCooldown -= Time.deltaTime;
                if (_currentDashCooldown <= 0f)
                {
                    DashStacks++;
                    _currentDashCooldown = DashCooldown;
                }
            }
            
            if (Input.GetKeyDown(SaveFile.Instance.GetKeybinding(KeyAction.Dash)))
            {
                UseDash();
            }
        }

        public void UseDash()
        {
            if (_dashStacks <= 0)
                return;
        
            DashStacks--;
            _dashTimeLeft = dashDuration;
            
            StartCoroutine(IFrames(0.2f));
            _isDashing = true;
        }

        private void FixedUpdate()
        {
            if (!_isDashing) return;
            
            if (_dashTimeLeft > 0)
            {
                var dashDirection = _transform.forward * dashSpeed;
                _characterController.Move(dashDirection * Time.fixedDeltaTime);
                _dashTimeLeft -= Time.fixedDeltaTime;
            }
            else
            {
                _isDashing = false;
            }
        }

        private IEnumerator IFrames(float iframeDuration)
        {
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("PlayerLayer"), LayerMask.NameToLayer("EnemyLayer"), true);
            yield return new WaitForSeconds(iframeDuration);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("PlayerLayer"), LayerMask.NameToLayer("EnemyLayer"), false);
        }
    }
}