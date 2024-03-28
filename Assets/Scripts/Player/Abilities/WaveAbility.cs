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
using RogueApeStudio.Crusader.Audio;

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
        [SerializeField] private AudioClip _waveSFX;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _transform;

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
                _animator.SetTrigger("WaveAbility");
                StartCooldownAsync(_cancellationTokenSource.Token);
            }
        }

        public void TriggerWaveAbilityEffects()
        {
            Instantiate(_Effect, _transform);
            AudioManager.instance.PlaySFX(_waveSFX, _transform, 1f);

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, _radius);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Enemy"))
                {
                    Vector3 knockbackDirection = _transform.position - hitCollider.transform.position;
                    knockbackDirection = knockbackDirection.normalized;
                    knockbackDirection = new Vector3(knockbackDirection.x, 0, knockbackDirection.z);

                    hitCollider.transform.GetComponent<Knockback>().AddKnockback(_knockbackForce, -knockbackDirection);
                }
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
                await UniTask.WaitUntil(() => CheckIfOnCooldown(), cancellationToken: token);
                CooldownAsync(_cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                Debug.LogError("Cooldown was Canceled");
            }
        }

        private bool CheckIfOnCooldown()
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

