using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RogueApeStudio.Crusader.Player.Abilities
{
    public class WaveAbility : MonoBehaviour
    {
        private CrusaderInputActions _actions;
        private InputAction _waveAbility;
        private bool _onCooldown = false;

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
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnEnable()
        {
            _waveAbility.started += OnWaveAbility;
            EnableWaveAbility();
        }

        private void OnWaveAbility(InputAction.CallbackContext context)
        {
            if (context.started && !_onCooldown)
            {
                _onCooldown = true;
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
                StartCooldown();
            }
        }

        private void EnableWaveAbility()
        {
            _waveAbility.Enable();
        }

        private async void StartCooldown()
        {
            await UniTask.WaitForSeconds(_cooldown);
            _onCooldown = false;
        }
    }
}

