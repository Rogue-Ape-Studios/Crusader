using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.PlasticSCM.Editor.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Rendering.Universal;

namespace RogueApeStudio.Crusader.Player.Combat
{
    public class BasicAttack : MonoBehaviour
    {
        private CrusaderInputActions _crusaderInputActions;
        private InputAction _attackInput;
        private InputAction _movementInput;
        private int _comboCounter = 0;
        [SerializeField] private Animator _animator;
        [SerializeField] private AnimationClip[] _animations;
        private float _delay = 0f;
        private bool _canAttack = true;

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
                switch (_comboCounter)
                {
                    case 1:
                        _animator.Play(_animations[_comboCounter - 1].name);
                        _delay = _animations[_comboCounter - 1].length;
                        break;
                    case 2:
                        _animator.Play(_animations[_comboCounter - 1].name);
                        _delay = _animations[_comboCounter - 1].length;
                        break;
                    case 3:
                        _animator.Play(_animations[_comboCounter - 1].name);
                        _delay = _animations[_comboCounter - 1].length;
                        _comboCounter = 0;
                        break;
                }
                Cooldown();
            }
        }

        private async void Cooldown()
        {
            _canAttack = false;
            await UniTask.WaitForSeconds(_delay);
            _canAttack = true;
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
