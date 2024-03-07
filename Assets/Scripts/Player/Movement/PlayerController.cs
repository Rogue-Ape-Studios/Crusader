using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Codice.CM.Client.Differences;

namespace RogueApeStudio.Crusader.Player.Movement
{
    public class PlayerController : MonoBehaviour
    {
        private CrusaderInputActions _crusaderInputActions;
        private InputAction _movementInput;
        private InputAction _dashInput;

        [SerializeField] private Rigidbody _rb;

        [Header("Movement Options"), SerializeField] private int _moveSpeed = 5;
        [SerializeField] private float _rotationSpeed;
        private Vector3 _targetDirection;
        private bool _canTurn = false;

        [Header("Dash Options"), SerializeField] private float _dashSpeed = 10f;
        [SerializeField] private float _dashDuration = 0.5f;
        [SerializeField] private float _dashCooldown = 1f;
        [SerializeField] private bool _isDashing = false;
        [SerializeField] private float _dashTimer = 0.5f;
        [SerializeField] private float _dashCooldownTimer = 0f;

        private void Awake()
        {
            _crusaderInputActions = new();
            _movementInput = _crusaderInputActions.Player.Move;
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
                Vector2 _inputDirection = _movementInput.ReadValue<Vector2>();
                Vector3 _dashDirection = Vector3.forward;

                _dashCooldownTimer = _dashCooldown;
                _isDashing = true;
                _dashTimer = _dashDuration;

                if (_inputDirection != Vector2.zero)
                {
                    _dashDirection = new Vector3(_inputDirection.x, 0f, _inputDirection.y).normalized;
                    // Rotate the player's body towards the target rotation
                    _rb.rotation = Quaternion.LookRotation(_dashDirection, Vector3.up);
                }

                Vector3 dashForce = _dashDirection * _dashSpeed;

                _rb.AddForce(dashForce, ForceMode.Impulse);


            }
        }

        private void OnMove()
        {
            Vector2 inputDirection = _movementInput.ReadValue<Vector2>();

            if (inputDirection != Vector2.zero && !_isDashing)
            {
                Vector3 movementDirection = new Vector3(inputDirection.x, 0f, inputDirection.y).normalized;
                if (Gamepad.current != null)
                {
                    float targetAngle = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg;
                    Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
                    // Smoothly rotate the player's body towards the target rotation
                    _rb.rotation = Quaternion.RotateTowards(_rb.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
                }
                // Calculate movement vector with the specified speed
                Vector3 _movement = _moveSpeed * Time.fixedDeltaTime * movementDirection;

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
            _movementInput.Enable();
        }

        private void DisableMovement()
        {
            _movementInput.Disable();
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
