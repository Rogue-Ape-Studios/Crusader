using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RogueApeStudio.Crusader.Player.Combat
{
    public class BasicAttack : MonoBehaviour
    {
        private CrusaderInputActions _crusaderInputActions;
        private InputAction _attackInput;
        private InputAction _movementInput;
        private int _comboCounter = 0;
        private bool _canAttack = true;
        private float _firstClickTime;
        private List<float> _clickTimes = new();
        [SerializeField] Animator _animator;
        private CancellationTokenSource _attackToken;
        private int _attackState = 0; // 0: Idle, 1: Attack 1, 2: Attack 2 (combo), etc.
        private const float _attackWindow = 0.5f; // Time window for combo attack

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

        //private void OnAttack(InputAction.CallbackContext obj)
        //{
        //    print("MEP MEP MEEEEEEEEEEEEEEP");
        //    float _timeDifference = 0f;
        //    float _attackCooldown = 1.2f;

        //    _clickTimes.Add(Time.time);

        //    // If there are at least two click times recorded
        //    if (_clickTimes.Count >= 2)
        //    {
        //        // Calculate the time difference between consecutive clicks
        //        _timeDifference = _clickTimes[^1] - _clickTimes[^2];
        //        print("Time difference between clicks " + (_clickTimes.Count - 1) + " and " + _clickTimes.Count + ": " + _timeDifference + " seconds");

        //        if (_timeDifference <= _attackCooldown)
        //            // Remove the first item from the list to keep it containing only the last two click times
        //            _clickTimes.RemoveAt(0);
        //        else _clickTimes.Clear();
        //    }

        //    if (_timeDifference <= _attackCooldown)
        //    {
        //        switch (_comboCounter)
        //        {
        //            case 0:
        //                print("attack 1");
        //                AttackComboAsync();
        //                break;
        //            case 1:
        //                print("attack 2");
        //                AttackComboAsync();
        //                break;
        //            case 2:
        //                print("attack 3");
        //                AttackComboAsync();
        //                break;
        //            default:
        //                return;

        //        }
        //    }
        //    else _comboCounter = 0;
        //}

        //private void AttackComboAsync()
        //{

        //    _animator.SetInteger("Attack", _comboCounter + 1);


        //    // Increment combo count, looping back to 0 after 2
        //    _comboCounter = (_comboCounter + 1) % 3;
        //    print(_comboCounter);


        //}

        //private float GetCurrentAnimationLengthInSeconds()
        //{
        //    // Get the current state information of the Animator for layer 0
        //    AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        //    // Get the length of the current animation clip in seconds
        //    float animationLength = _animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;

        //    // Calculate the full length of the animation in seconds based on normalized time
        //    float fullAnimationLength = animationLength / stateInfo.speed;

        //    return fullAnimationLength;
        //}

        private async void Start()
        {
            

            while (true)
            {
                await UniTask.Yield(PlayerLoopTiming.Update);

                if (_attackToken != null && !_attackToken.IsCancellationRequested)
                {
                    // Reset attack state if the attack button hasn't been pressed within the time window
                    _attackToken.Cancel();
                    _attackToken.Dispose();
                    _attackState = 0;
                    _animator.SetInteger("Attack", _attackState);
                }
            }
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                if (_attackToken != null && !_attackToken.IsCancellationRequested)
                {
                    // If the attack button is pressed within the time window, restart the window
                    _attackToken.Cancel();
                    _attackToken.Dispose();
                }

                _attackToken = new CancellationTokenSource();

                switch (_attackState)
                {
                    case 0:
                        print("hello");
                        _attackState = 1;
                        break;
                    case 1:
                        _attackState = 2; 
                        break;
                    case 2:
                        _attackState = 3;
                        break;
                    default:
                        _attackState = 1; // Reset to first attack in combo if button pressed during window
                        break;
                }

                _animator.SetInteger("Attack", _attackState);

                // Start a new window for the next attack
                UniTask.Delay(TimeSpan.FromSeconds(_attackWindow), cancellationToken: _attackToken.Token).Forget();
            }
        }


        private void FixedUpdate()
        {

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
