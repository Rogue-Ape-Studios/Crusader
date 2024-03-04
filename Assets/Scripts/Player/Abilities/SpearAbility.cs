using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace RogueApeStudio.Crusader.Player.Abilities
{
    public class SpearAbility : MonoBehaviour
    {
        private CrusaderInputActions _actions;
        private InputAction _spearAbility;
        private Vector3 _direction;
        private RaycastHit _cameraRayHit;
        private bool _onCooldown = false;

        [SerializeField] private Rigidbody _spear;
        [SerializeField] private Camera _cam;
        [SerializeField] private float _cooldown;
        [SerializeField] private float _speed;

        private void Awake()
        {
            _actions = new();
            _spearAbility = _actions.Player.Ability_1;
        }

        private void OnEnable()
        {
            _spearAbility.canceled += ReleaseSpearAbility;
            EnableWaveAbility();
        }



        private void Update()
        {
            Ray cameraRay = _cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(cameraRay, out _cameraRayHit))
            {
                if (_cameraRayHit.transform.tag == "Ground")
                {
                    Vector3 targetPosition = new Vector3(_cameraRayHit.point.x, 0, _cameraRayHit.point.z);
                    _direction = targetPosition - transform.position;
                    _direction.Normalize();
                }
            }
        }

        private void ReleaseSpearAbility(InputAction.CallbackContext context)
        {
            if (!_onCooldown)
            {
                Rigidbody spear = Instantiate(_spear,
                    new Vector3(transform.position.x, 1, transform.position.z),
                    Quaternion.LookRotation(_direction));
                spear.AddForce(_direction * _speed, ForceMode.Impulse);

                StartCooldown();
            }
        }

        private void EnableWaveAbility()
        {
            _spearAbility.Enable();
        }

        private async void StartCooldown()
        {
            _onCooldown = true;
            await UniTask.WaitForSeconds(_cooldown);
            _onCooldown = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            //if enemy trigger dmg here
        }
    }
}

