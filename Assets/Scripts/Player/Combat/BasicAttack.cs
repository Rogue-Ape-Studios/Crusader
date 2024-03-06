using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using RogueApeStudio.Crusader.Input;

namespace RogueApeStudio.Crusader.Player.Combat
{
    public class BasicAttack : MonoBehaviour
    {
        private CrusaderInputActions _crusaderInputActions;
        private InputAction _attackInput;
        private InputAction _movementInput;
        [SerializeField] private int _comboCounter = 0;
        private float _delay = 0f;
        [SerializeField] private float _attackWindow = 0.5f;
        [SerializeField] private bool _canAttack = true;
        [SerializeField] private bool _windowCountdown = false;
        [SerializeField] private Animator _animator;
        [SerializeField] private AnimationClip[] _animations;

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
                Attack();
                Cooldown();
            }
        }

        private void Attack()
        {
            _animator.Play(_animations[_comboCounter - 1].name);
            _delay = _animations[_comboCounter - 1].length / 5;
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
