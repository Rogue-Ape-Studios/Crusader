using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using RogueApeStudio.Crusader.Input;

namespace RogueApeStudio.Crusader.Player.Combat
{
    public class BasicAttack : MonoBehaviour
    {
        private CrusaderInputActions _crusaderInputActions;
        private InputAction _attackInput;
        private InputAction _movementInput;

        [SerializeField] private Animator _animator;
        [SerializeField] private AnimationClip[] _animations;
        [SerializeField] private Camera _cam;
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private int _comboCounter = 0;
        [SerializeField] private int _attackSpeed = 5;
        [SerializeField] private float _rotationSpeed = 1000;
        [SerializeField] private float _attackWindow = 0.5f;
        [SerializeField] private bool _canAttack = true;
        [SerializeField] private bool _windowCountdown = false;
        [SerializeField] private string[] _clickableTags;

        private float _delay = 0f;
        private RaycastHit _cameraRayHit;
        private Vector3 _targetDirection;
        private bool _isTurning = false;

        private void Awake()
        {
            _crusaderInputActions = new();
            _attackInput = _crusaderInputActions.Player.BasicAttack;
            _movementInput = _crusaderInputActions.Player.Move;
        }

        private void OnEnable()
        {
            _attackInput.started += OnAttack;
            EnableBasicAttack();
            DisableMovement();
        }

        private void OnDisable()
        {
            _attackInput.started -= OnAttack;
            DisableBasicAttack();
            EnableMovement();
        }

        private void OnAttack(InputAction.CallbackContext context)
        {
            if (context.started && _canAttack)
            {
                _comboCounter++;
                if (Keyboard.current != null) 
                    TurnDirection();
                Attack();
                Cooldown();
            }
        }

        private void TurnDirection()
        {
            Ray cameraRay = _cam.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(cameraRay, out _cameraRayHit))
            {
                if (_clickableTags.Any(tag => _cameraRayHit.transform.CompareTag(tag)))
                {
                    Vector3 targetPosition = new(_cameraRayHit.point.x, 0, _cameraRayHit.point.z);
                    _targetDirection = targetPosition - transform.position;
                    _targetDirection.y = 0;
                    _targetDirection.Normalize();
                    _isTurning = true;
                }
            }
        }

        private void Attack()
        {
            _animator.Play(_animations[_comboCounter - 1].name);
            _delay = _animations[_comboCounter - 1].length / _attackSpeed;
            _attackWindow = 0.5f;
            _windowCountdown = true;
            if (_comboCounter == 3)
                _comboCounter = 0;
        }

        private async void Cooldown()
        {
            _canAttack = false;
            await UniTask.WaitForSeconds(_delay);
            _canAttack = true;
        }

        private void Update()
        {
            if (_canAttack && _windowCountdown)
            {
                _attackWindow -= Time.deltaTime;
            }

            if (_attackWindow <= 0)
            {
                _attackWindow = 0.5f;
                _comboCounter = 0;
                _animator.Play("Idle");
                _windowCountdown = false;
            }

            if (_isTurning)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_targetDirection, Vector3.up);
                _rb.MoveRotation(Quaternion.RotateTowards(_rb.rotation, targetRotation, _rotationSpeed * Time.deltaTime));

                // Check if the player has reached the desired rotation
                if (Quaternion.Angle(_rb.rotation, targetRotation) < 0.1f)
                {
                    _isTurning = false;
                }
            }
        }

        private void EnableBasicAttack()
        {
            _attackInput.Enable();
        }

        private void DisableBasicAttack()
        {
            _attackInput?.Disable();
        }
        private void DisableMovement()
        {
            _movementInput.Disable();
        }

        private void EnableMovement()
        {
            _movementInput.Enable();
        }
    }
}
