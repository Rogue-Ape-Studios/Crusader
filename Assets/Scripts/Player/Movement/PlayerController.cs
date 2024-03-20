using UnityEngine;
using UnityEngine.InputSystem;
using RogueApeStudio.Crusader.Input;
using RogueApeStudio.Crusader.HealthSystem;
using System.Linq;
using UnityEngine.EventSystems;
using System.Threading;

namespace RogueApeStudio.Crusader.Player.Movement
{
    public class PlayerController : MonoBehaviour
    {
        private CrusaderInputActions _crusaderInputActions;
        private InputAction _movementInput;
        private InputAction _dashInput;

        private Vector3 _lastMovementDirection = Vector3.zero;
        private Vector3 _lastLookDirection = Vector3.zero;
        private RaycastHit _cameraRayHit;
        [SerializeField] private bool _readInputs = true;

        [SerializeField] private Rigidbody _rb;
        [SerializeField] private Transform _transform;
        [SerializeField] private Animator _animator;

        [Header("Movement Options")]
        [SerializeField] private int _moveSpeed = 5;
        [SerializeField] private float _rotationSpeed;

        [Header("Dash Options")]
        [SerializeField] private string[] _invulnerableLayers;
        [SerializeField] private float _dashSpeed = 10f;
        [SerializeField] private float _dashDuration = 0.5f;
        [SerializeField] private float _dashCooldown = 1f;
        [SerializeField] private bool _isDashing = false;
        [SerializeField] private float _dashTimer = 0.5f;
        [SerializeField] private float _dashCooldownTimer = 0.5f;

        private void Awake()
        {
            _crusaderInputActions = new();
            _movementInput = _crusaderInputActions.Player.Move;
            _dashInput = _crusaderInputActions.Player.Dash;
        }

        private void OnEnable()
        {
            _dashInput.performed += OnDash;
            EnableDash();
            EnableMovement();
        }

        private void OnDisable()
        {
            DisableMovement();
            DisableDash();
            _dashInput.performed -= OnDash;
        }


        private void OnDash(InputAction.CallbackContext context)
        {
            if (!_isDashing && _dashCooldownTimer <= 0 && _readInputs && !_animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerDiveForward"))
            {
                print("DAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAASHIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIING");

                _dashCooldownTimer = _dashCooldown;
                _animator.SetTrigger("Dash");
                _isDashing = true;
                SetReadInput(false);

                Vector3 dashForce = _transform.forward * _dashSpeed;
                _rb.AddForce(dashForce, ForceMode.Impulse);
                _rb.excludeLayers = LayerMask.GetMask(_invulnerableLayers);
            }
        }

        private void OnMove()
        {
            Vector2 inputDirection = _movementInput.ReadValue<Vector2>();

            if (inputDirection != Vector2.zero && !_isDashing && _readInputs)
            {
                Vector3 movementDirection = new Vector3(inputDirection.x, 0f, inputDirection.y).normalized;

                TurnPlayer(movementDirection);

                Vector3 _movement = _moveSpeed * Time.fixedDeltaTime * movementDirection;
                _lastMovementDirection = movementDirection;

                _animator.SetFloat("Speed", 1f);

                _rb.MovePosition(_rb.position + _movement);
            }
            else
            {
                _animator.SetFloat("Speed", 0f);
            }

        }

        private void TurnPlayer(Vector3 direction)
        {
            if (_isDashing)
            {
                _transform.rotation = Quaternion.LookRotation(direction);
            }
            else
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);

                _rb.rotation = Quaternion.RotateTowards(_rb.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            }
        }

        private void HandleDashTimers()
        {
            if (_dashCooldownTimer > 0 && !_isDashing) _dashCooldownTimer -= Time.fixedDeltaTime;

            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerDiveForward") && _isDashing)
            {
                print("NO LONGER DAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAASHIIIIIIIIIIIIIIIIIIIIIIIING");
                _isDashing = false;
                SetReadInput(true);
                _rb.excludeLayers = LayerMask.GetMask("");

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

        public void SetReadInput(bool readInput)
        {
            _readInputs = readInput;
        }

        public void AddForce(float force)
        {
            SetReadInput(false);
            Vector3 forceDirection = _transform.forward * force;
            _rb.AddForce(forceDirection, ForceMode.Impulse);
        }

        public void ToggleInputActions()
        {
            _crusaderInputActions.Player.Disable();
            _crusaderInputActions.UI.Enable();
        }
    }
}
