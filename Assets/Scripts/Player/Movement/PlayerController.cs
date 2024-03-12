using UnityEngine;
using UnityEngine.InputSystem;
using RogueApeStudio.Crusader.Input;
using System.Linq;
using UnityEngine.EventSystems;

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
        private bool _readInputs = true;

        [SerializeField] private Rigidbody _rb;
        [SerializeField] private Camera _cam;
        [SerializeField] private Animator _animator;
        [SerializeField] private string[] _tags;

        [Header("Movement Options")]
        [SerializeField] private int _moveSpeed = 5;
        [SerializeField] private float _rotationSpeed;

        [Header("Dash Options")]
        [SerializeField] private float _dashSpeed = 10f;
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
            if (!_isDashing && _dashCooldownTimer <= 0 && _readInputs)
            {
                Vector2 inputDirection = _movementInput.ReadValue<Vector2>();
                Vector3 dashDirection = Vector3.forward;

                _dashCooldownTimer = _dashCooldown;
                _isDashing = true;
                _dashTimer = _dashDuration;

                if (inputDirection != Vector2.zero)
                {
                    dashDirection = new Vector3(inputDirection.x, 0f, inputDirection.y).normalized;

                    TurnPlayer(dashDirection);
                }
                else
                {
                    if (_lastMovementDirection != Vector3.zero)
                    {
                        dashDirection = _lastLookDirection.normalized;
                    }
                }

                Vector3 dashForce = dashDirection * _dashSpeed;

                _rb.AddForce(dashForce, ForceMode.Impulse);


            }
        }

        private void OnMove()
        {
            Vector2 inputDirection = _movementInput.ReadValue<Vector2>();

            if (inputDirection != Vector2.zero && !_isDashing && _readInputs)
            {
                Vector3 movementDirection = new Vector3(inputDirection.x, 0f, inputDirection.y).normalized;
                if (Gamepad.current != null)
                {
                    TurnPlayer(movementDirection);
                }

                Vector3 _movement = _moveSpeed * Time.fixedDeltaTime * movementDirection;
                _lastMovementDirection = movementDirection;


                _rb.MovePosition(_rb.position + _movement);
            }
        }

        private void TurnPlayer(Vector3 direction)
        {
            if (_isDashing)
            {
                _rb.transform.rotation = Quaternion.LookRotation(direction);
            }
            else if (Gamepad.current != null && !_isDashing)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);

                _rb.rotation = Quaternion.RotateTowards(_rb.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            }
        }

        private void TurnPlayer()
        {
            if (Keyboard.current != null)
            {
                Ray cameraRay = _cam.ScreenPointToRay(Mouse.current.position.ReadValue());

                if (Physics.Raycast(cameraRay, out _cameraRayHit))
                {
                    if (_tags.Any(tag => _cameraRayHit.transform.CompareTag(tag)))
                    {
                        Vector3 targetPosition = new(_cameraRayHit.point.x, 0, _cameraRayHit.point.z);
                        _rb.transform.LookAt(targetPosition);
                        _rb.transform.rotation = Quaternion.Euler(0, _rb.transform.rotation.eulerAngles.y, 0);
                        _lastLookDirection = targetPosition - _rb.transform.position;
                        _lastLookDirection.y = 0f;
                    }
                }
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

        public void AddForce(float force)
        {
            SetReadInput(false);
            Vector3 forceDirection = _lastLookDirection.normalized * force;
            _rb.AddForce(forceDirection, ForceMode.Impulse);
        }

        public void SetReadInput(bool readInput)
        {
            _readInputs = readInput;
        }

        private void HandleRunningAnimation()
        {
            Vector2 inputDirection = _movementInput.ReadValue<Vector2>();

            Vector3 moveDirection = new(inputDirection.x, 0f, inputDirection.y);

            Vector3 currentEulerAngles = _rb.transform.rotation.eulerAngles;

            // Get the rotation around the y-axis
            float currentRotation = currentEulerAngles.y;
            Debug.Log(inputDirection.x + "    " + inputDirection.y );

            // Check input direction for forward, backward, left, or right movement
            if (inputDirection.y > 0)
            {
                if ((currentRotation >= 315 && currentRotation <= 360) || (currentRotation >= 0 && currentRotation <= 45))
                {
                    // Forward animation
                    _animator.SetFloat("Vertical", 1f);
                    _animator.SetFloat("Horizontal", 0f);
                }
                else if (currentRotation >= 45 && currentRotation <= 135)
                {
                    // Right animation
                    _animator.SetFloat("Vertical", 0f);
                    _animator.SetFloat("Horizontal", 1f);
                }
                else if (currentRotation >= 135 && currentRotation <= 225)
                {
                    // Backward animation
                    _animator.SetFloat("Vertical", -1f);
                    _animator.SetFloat("Horizontal", 0f);
                }
                else if (currentRotation >= 225 && currentRotation <= 315)
                {
                    // Left animation
                    _animator.SetFloat("Vertical", 0f);
                    _animator.SetFloat("Horizontal", -1f);
                }
            }
            else if (inputDirection.y < 0)
            {
                if ((currentRotation >= 315 && currentRotation <= 360) || (currentRotation >= 0 && currentRotation <= 45))
                {
                    // Backward animation
                    _animator.SetFloat("Vertical", -1f);
                    _animator.SetFloat("Horizontal", 0f);
                }
                else if (currentRotation >= 45 && currentRotation <= 135)
                {
                    // Left animation
                    _animator.SetFloat("Vertical", 0f);
                    _animator.SetFloat("Horizontal", -1f);
                }
                else if (currentRotation >= 135 && currentRotation <= 225)
                {
                    // Forward animation
                    _animator.SetFloat("Vertical", 1f);
                    _animator.SetFloat("Horizontal", 0f);
                }
                else if (currentRotation >= 225 && currentRotation <= 315)
                {
                    // Right animation
                    _animator.SetFloat("Vertical", 0f);
                    _animator.SetFloat("Horizontal", 1f);
                }
            }
            else if (inputDirection.x < 0)
            {
                if ((currentRotation >= 315 && currentRotation <= 360) || (currentRotation >= 0 && currentRotation <= 45))
                {
                    // Right animation
                    _animator.SetFloat("Vertical", 0f);
                    _animator.SetFloat("Horizontal", 1f);
                }
                else if (currentRotation >= 45 && currentRotation <= 135)
                {
                    // Backward animation
                    _animator.SetFloat("Vertical", -1f);
                    _animator.SetFloat("Horizontal", 0f);
                }
                else if (currentRotation >= 135 && currentRotation <= 225)
                {
                    // Left animation
                    _animator.SetFloat("Vertical", 0f);
                    _animator.SetFloat("Horizontal", -1f);
                }
                else if (currentRotation >= 225 && currentRotation <= 315)
                {
                    // Forward animation
                    _animator.SetFloat("Vertical", 1f);
                    _animator.SetFloat("Horizontal", 0f);
                }
            }
            else if (inputDirection.x > 0)
            {
                if ((currentRotation >= 315 && currentRotation <= 360) || (currentRotation >= 0 && currentRotation <= 45))
                {
                    // Left animation
                    _animator.SetFloat("Vertical", 0f);
                    _animator.SetFloat("Horizontal", -1f);
                }
                else if (currentRotation >= 45 && currentRotation <= 135)
                {
                    // Forward animation
                    _animator.SetFloat("Vertical", 1f);
                    _animator.SetFloat("Horizontal", 0f);
                }
                else if (currentRotation >= 135 && currentRotation <= 225)
                {
                    // Right animation
                    _animator.SetFloat("Vertical", 0f);
                    _animator.SetFloat("Horizontal", 1f);
                }
                else if (currentRotation >= 225 && currentRotation <= 315)
                {
                    // Backward animation
                    _animator.SetFloat("Vertical", -1f);
                    _animator.SetFloat("Horizontal", 0f);
                }
            }
        }

        private void FixedUpdate()
        {
            OnMove();
            HandleDashTimers();
        }

        private void Update()
        {
            if (!_isDashing && _readInputs)
                TurnPlayer();

            if (_movementInput.ReadValue<Vector2>() != Vector2.zero)
                HandleRunningAnimation();
            else 
            {
                _animator.SetFloat("Vertical", 0f);
                _animator.SetFloat("Horizontal", 0f);
            }
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
