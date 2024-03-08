using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using RogueApeStudio.Crusader.Input;
using System.Threading;

namespace RogueApeStudio.Crusader.Player.Abilities
{
    public class WaveAbility : MonoBehaviour
    {
        private CrusaderInputActions _actions;
        private InputAction _waveAbility;
        private bool _onCooldown = false;
        private CancellationTokenSource _cancellationTokenSource;

        [SerializeField] private float _radius;
        [SerializeField] private float _knockbackForce;
        [SerializeField] private float _cooldown;
        [SerializeField] private Transform _root;
        [SerializeField] private GameObject _Effect;

        // Start is called before the first frame update
        void Awake()
        {
            _actions = new();
            _waveAbility = _actions.Player.Ability_2;
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

        private void OnWaveAbility(InputAction.CallbackContext context)
        {
            if (context.started && !_onCooldown)
            {
                Instantiate(_Effect, gameObject.transform);

                Collider[] hitColliders = Physics.OverlapSphere(_root.position, _radius);
                foreach (var hitCollider in hitColliders)
                {
                    if (hitCollider.CompareTag("Enemy"))
                    {
                        Vector3 knockbackDiraction = _root.position - hitCollider.transform.position;
                        knockbackDiraction = knockbackDiraction.normalized;
                        knockbackDiraction = new Vector3(knockbackDiraction.x, 0, knockbackDiraction.z);

                        float force = (_knockbackForce + (1 / Vector3.Distance(_root.transform.position, hitCollider.transform.position) * 10));

                        hitCollider.transform.GetComponent<Rigidbody>().AddForce(-knockbackDiraction * force, ForceMode.Impulse);
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
                _onCooldown = true;
                await UniTask.WaitForSeconds(_cooldown, cancellationToken: token);
                _onCooldown = false;
            }
            catch (OperationCanceledException)
            {
                Debug.LogError("Cooldown was Canceled");
            }
        }
    }
}

