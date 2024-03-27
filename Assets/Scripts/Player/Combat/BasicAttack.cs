using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using RogueApeStudio.Crusader.Input;
using RogueApeStudio.Crusader.Player.Movement;
using RogueApeStudio.Crusader.Audio;
using System.Threading;
using System;
using RogueApeStudio.Crusader.Items;

namespace RogueApeStudio.Crusader.Player.Combat
{
    public class BasicAttack : MonoBehaviour
    {
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private float _force = 10f;
        [SerializeField] private Rigidbody _rb;

        [Header("Player preferences")]
        [SerializeField] bool _cursorDirection = true;

        [Header("Raycast necessities")]
        [SerializeField] private Camera _cam;
        [SerializeField] private string[] _tags;

        [Header("Animation")]
        [SerializeField] private Animator _animator;
        [SerializeField] private AnimationClip[] _animations;
        [SerializeField] private Collider _sword;
        [SerializeField] private GameObject[] _vfx;

        [Header("Attack Info")]
        [SerializeField] private int _comboCounter = 0;
        [SerializeField] private float _attackSpeed = 5;
        [SerializeField] private float _attackWindow = 0.5f;
        [SerializeField] private bool _canAttack = true;
        [SerializeField] private bool _windowCountdown = false;

        [Header("Sword Swings SFX")]
        [SerializeField] private AudioClip[] _swingSoundClips;
        
        [Header("Dependencies")]
        [SerializeField] private PlayerScriptableObject _playerStats;
        private CrusaderInputActions _crusaderInputActions;
        private InputAction _attackInput;
        private RaycastHit _cameraRayHit;
        private float _delay = 0f;
        private CancellationTokenSource _cancellationTokenSource;
        private float _baseAttackSpeed;

        private void Awake()
        {
            _crusaderInputActions = new();
            _attackInput = _crusaderInputActions.Player.BasicAttack;
            _cancellationTokenSource = new CancellationTokenSource();
            _baseAttackSpeed = _attackSpeed;
        }

        private void OnEnable()
        {
            _attackInput.performed += OnAttack;
            EnableBasicAttack();
        }


        private void OnDisable()
        {
            _attackInput.performed -= OnAttack;
            DisableBasicAttack();
        }

        private void OnDestroy()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        private void OnAttack(InputAction.CallbackContext context)
        {
            if (_canAttack && !_playerController.GetIsDashing())
            {
                _comboCounter++;
                Attack();
                if (_comboCounter == 3)
                    _playerController.AddForce(_force);
                StartCooldownAsync(_cancellationTokenSource.Token);
                AudioManager.instance.PlayRandomSwordSFX(_swingSoundClips, transform, 1f);
                if (_comboCounter >= 3)
                {
                    _attackSpeed--;
                    _comboCounter = 0;
                }
            }

        }

        private void Attack()
        {
            if (_comboCounter == 2)
                _attackSpeed++;
            _sword.enabled = true;
            playVFX(_comboCounter);
            _animator.Play(_animations[_comboCounter - 1].name);
            _delay = _animations[_comboCounter - 1].length / _attackSpeed;
            _attackWindow = 0.5f;
            
            if (Keyboard.current != null && _cursorDirection)
            {
                Ray cameraRay = _cam.ScreenPointToRay(Mouse.current.position.ReadValue());

                if (Physics.Raycast(cameraRay, out _cameraRayHit))
                {
                    if (_tags.Any(tag => _cameraRayHit.transform.CompareTag(tag)))
                    {
                        Vector3 targetPosition = new(_cameraRayHit.point.x, 0, _cameraRayHit.point.z);
                        Vector3 direction = targetPosition - _rb.transform.position;
                        direction.Normalize();
                        direction.y = 0f;
                        _rb.transform.rotation = Quaternion.LookRotation(direction);
                    }
                }
            }
            _windowCountdown = true;
            _windowCountdown = true;
        }

        private async void StartCooldownAsync(CancellationToken token)
        {
            try
            {
                _canAttack = false;
                _playerController.SetReadInput(false);
                await UniTask.WaitForSeconds(_delay, cancellationToken: token);
                _rb.velocity = Vector3.zero;
                _sword.enabled = false;
                _canAttack = true;
            }
            catch (OperationCanceledException)
            {
                Debug.LogError("Cooldown was Canceled because operation was cancelled");
            }
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
                _attackSpeed = _baseAttackSpeed;
                _animator.Play("Movement");
                _windowCountdown = false;
            }
            else if (_attackWindow < 0.4f)
                _playerController.SetReadInput(true);
        }

        private void EnableBasicAttack()
        {
            _attackInput.Enable();
        }

        private void DisableBasicAttack()
        {
            _attackInput?.Disable();
        }

        public void SetCanAttack(bool canAttack)
        {
            _canAttack = canAttack;
        }

        private void playVFX(int counter)
        {
            switch (counter)
            {
                case 1:
                    DelayAsync(_cancellationTokenSource.Token, 0.2f, _vfx[0]);
                    break;
                case 2:
                    DelayAsync(_cancellationTokenSource.Token, 0.3f, _vfx[1]);
                    break;
                case 3:
                    DelayAsync(_cancellationTokenSource.Token, 0.2f, _vfx[2]);
                    break;
            }
        }

        private async void DelayAsync(CancellationToken token, float delay, GameObject vfx)
        {
            try
            {
                await UniTask.WaitForSeconds(delay, cancellationToken: token);
                vfx.SetActive(false);
                vfx.SetActive(true);
            }
            catch (OperationCanceledException)
            {
                Debug.LogError("Cooldown was Canceled because operation was cancelled");
            }
        }
    }
}
