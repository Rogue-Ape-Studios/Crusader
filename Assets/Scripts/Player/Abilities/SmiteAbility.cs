using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using RogueApeStudio.Crusader.Input;
using System.Threading;
using RogueApeStudio.Crusader.UI.Cooldown;
using RogueApeStudio.Crusader.Audio;

namespace RogueApeStudio.Crusader.Player.Abilities
{
    public class SmiteAbility : MonoBehaviour
    {
        private CrusaderInputActions _actions;
        private InputAction _smiteAbility;
        private Vector3 _direction;
        private RaycastHit _cameraRayHit;
        private bool _onCooldown = false;
        private CancellationTokenSource _cancellationTokenSource;
        //charges will come from the PlayerStats (item system)
        private int _charges = 1;
        private int _remainingCooldown;
        private bool _startedUI = false;

        [SerializeField] private GameObject _sword;
        [SerializeField] private Camera _cam;
        [SerializeField] private int _cooldown;
        [SerializeField] private AbilityCooldown _cooldownUI;
        [SerializeField] private AudioClip _smiteSFX;

        private void Awake()
        {
            _actions = new();
            _smiteAbility = _actions.Player.Ability_3;
            _cooldownUI.GetCharges(_charges);
            _cancellationTokenSource = new CancellationTokenSource();
        }

        private void OnEnable()
        {
            _smiteAbility.started += OnSmiteAbility;
            EnableWaveAbility();
        }

        private void OnDestroy()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        private void Update()
        {
            Ray cameraRay = _cam.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(cameraRay, out _cameraRayHit))
            {
                if (_cameraRayHit.transform.tag == "Ground")
                {
                    Vector3 targetPosition = new Vector3(_cameraRayHit.point.x, 1, _cameraRayHit.point.z);
                    _direction = targetPosition - transform.position;
                    _direction.Normalize();
                }
            }

            if (_charges == 0 && !_startedUI)
            {
                _startedUI = true;
                _cooldownUI.StartCooldown(_remainingCooldown);
            }

            if (_cooldownUI.IsCooldownEnded())
            {
                _startedUI = false;
            }
        }

        private void OnSmiteAbility(InputAction.CallbackContext context)
        {
            if (_charges != 0)
            {
                GameObject sword = Instantiate(_sword,
                    new Vector3(transform.position.x, 1, transform.position.z),
                    Quaternion.LookRotation(_direction));
                AudioManager.instance.PlaySFX(_smiteSFX, transform, 1f);

                StartCooldownAsync(_cancellationTokenSource.Token);
            }
        }

        private void EnableWaveAbility()
        {
            _smiteAbility.Enable();
        }

        private async void StartCooldownAsync(CancellationToken token)
        {
            try
            {
                _charges--;
                _cooldownUI.GetCharges(_charges);
                await UniTask.WaitUntil(() => checkIfOnCooldown(), cancellationToken: token);
                CooldownAsync(_cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                Debug.LogError("Cooldown was Canceled");
            }
        }

        private bool checkIfOnCooldown()
        {
            return !_onCooldown;
        }

        private async void CooldownAsync(CancellationToken token)
        {
            try
            {
                _remainingCooldown = _cooldown;
                _onCooldown = true;
                for (int i = 0; i < _cooldown; i++)
                {
                    await UniTask.WaitForSeconds(1, cancellationToken: token);
                    _remainingCooldown--;
                }
                _charges++;
                _cooldownUI.GetCharges(_charges);
                _onCooldown = false;
            }
            catch (OperationCanceledException)
            {
                Debug.LogError("Cooldown was Canceled");
            }

        }
    }
}

