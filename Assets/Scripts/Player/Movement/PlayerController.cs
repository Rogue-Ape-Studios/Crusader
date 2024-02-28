using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RogueApeStudio.Crusader.Player.Movement
{
    public class PlayerController : MonoBehaviour
    {
        private CrusaderInputActions _crusaderInputActions;
        private InputAction _moventInput;
        private InputAction _dashInput;

        [SerializeField] private Rigidbody _rb;

        [Header("Movement Options"), SerializeField] private int _moveSpeed = 5;

        [Header("Dash Options"), SerializeField] private float _dashSpeed = 10f;
        [SerializeField] private float _dashDuration = 0.5f;
        [SerializeField] private float _dashCooldown = 1f;
        [SerializeField] private bool _isDashing = false;
        [SerializeField] private float _dashTimer = 0.5f;
        [SerializeField] private float _dashCooldownTimer = 0f;

        private void Awake()
        {
            _crusaderInputActions = new();
            _moventInput = _crusaderInputActions.Player.Move;
            _dashInput = _crusaderInputActions.Player.Dash;
        }

        private void OnEnable()
        {
            _dashInput.started += OnDash;
            EnableDash();
            EnableMovement();
        }

        private void OnDisable()
        {
            DisableMovement();
            DisableDash();
            _dashInput.started -= OnDash;
        }

        private void OnDash(InputAction.CallbackContext context)
        {
            if (context.started && !_isDashing && _dashCooldownTimer <= 0)
            {
                Vector2 _inputDirection = _moventInput.ReadValue<Vector2>();
                Vector3 _dashDirection = Vector3.forward;

                _dashCooldownTimer = _dashCooldown;
                _isDashing = true;
                _dashTimer = _dashDuration;

                if (_inputDirection != Vector2.zero)
                {
                    _dashDirection = new Vector3(_inputDirection.x, 0f, _inputDirection.y).normalized;
                }

                Vector3 dashForce = _dashDirection * _dashSpeed;

               _rb.AddForce(dashForce, ForceMode.Impulse);
                

            }
        }

        private void OnMove()
        {
            Vector2 _inputDirection = _moventInput.ReadValue<Vector2>();

            if (_inputDirection != Vector2.zero && !_isDashing)
            {
                Vector3 _movementDirection = new Vector3(_inputDirection.x, 0f, _inputDirection.y).normalized;

                // Calculate movement vector with the specified speed
                Vector3 _movement = _moveSpeed * Time.fixedDeltaTime * _movementDirection;

                // Move the character using Rigidbody
                _rb.MovePosition(_rb.position + _movement);
            }
        }

        private void HandleDashTimers()
        {
          
            if (_dashCooldownTimer > 0) _dashCooldownTimer -= Time.fixedDeltaTime;

            if (_dashTimer <= 0 && _isDashing)
            {

                _isDashing = false;
                _rb.velocity = Vector3.zero;

            }
            else if (_isDashing) _dashTimer -= Time.fixedDeltaTime;
        }

        private void FixedUpdate()
        {
            OnMove();
            HandleDashTimers();
        }

        private void EnableMovement()
        {
            _moventInput.Enable();
        }

        private void DisableMovement()
        {
            _moventInput.Disable();
        }
        private void EnableDash()
        {
            _dashInput.Enable();
        }

        private void DisableDash()
        {
            _dashInput.Disable();
        }

    }
}
