using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using RogueApeStudio.Crusader.Input;
using System.Threading;
using RogueApeStudio.Crusader.UI.Cooldown;
using RogueApeStudio.Crusader.Units.Knockback;

namespace RogueApeStudio.Crusader.Player.Abilities
{
    public class WaveAbility : MonoBehaviour
    {
        private CrusaderInputActions _actions;
        private InputAction _waveAbility;
        private bool _onCooldown = false;
        private CancellationTokenSource _cancellationTokenSource;
        //charges will come from the PlayerStats (item system)
        private int _charges = 1;
        private int _remainingCooldown;
        private bool _startedUI = false;

        [SerializeField] private float _radius;
        [SerializeField] private float _knockbackForce;
        [SerializeField] private int _cooldown;
        [SerializeField] private GameObject _Effect;
        [SerializeField] private AbilityCooldown _cooldownUI;

        // Start is called before the first frame update
        void Awake()
        {
            _actions = new();
            _waveAbility = _actions.Player.Ability_2;
            _cooldownUI.GetCharges(_charges);
            _cancellationTokenSource = new CancellationTokenSource();
        }

        private void OnEnable()
        {
            _waveAbility.started += OnWaveAbility;
            EnableWaveAbility();
        }

        private void OnDestroy()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        private void Update()
        {
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

        private void OnWaveAbility(InputAction.CallbackContext context)
        {
            if (_charges != 0)
            {
                Instantiate(_Effect, gameObject.transform);

                Collider[] hitColliders = Physics.OverlapSphere(transform.position, _radius);
                foreach (var hitCollider in hitColliders)
                {
                    if (hitCollider.CompareTag("Enemy"))
                    {
                        Vector3 knockbackDiraction = transform.position - hitCollider.transform.position;
                        knockbackDiraction = knockbackDiraction.normalized;
                        knockbackDiraction = new Vector3(knockbackDiraction.x, 0, knockbackDiraction.z);

                        float force = (_knockbackForce + (1 / Vector3.Distance(transform.transform.position, hitCollider.transform.position) * 10));

                        hitCollider.transform.GetComponent<Knockback>().AddKnockback(force, -knockbackDiraction);
                    }
                }
                StartCooldownAsync(_cancellationTokenSource.Token);
            }
        }

        private void EnableWaveAbility()
        {
            _waveAbility.Enable();
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

