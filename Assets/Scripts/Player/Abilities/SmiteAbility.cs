using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using RogueApeStudio.Crusader.Input;

namespace RogueApeStudio.Crusader.Player.Abilities
{
    public class SmiteAbility : MonoBehaviour
    {
        private CrusaderInputActions _actions;
        private InputAction _smiteAbility;
        private Vector3 _direction;
        private RaycastHit _cameraRayHit;
        private bool _onCooldown = false;

        [SerializeField] private GameObject _sword;
        [SerializeField] private Camera _cam;
        [SerializeField] private float _cooldown;

        private void Awake()
        {
            _actions = new();
            _smiteAbility = _actions.Player.Ability_3;
        }

        private void OnEnable()
        {
            _smiteAbility.started += OnSmiteAbility;
            EnableWaveAbility();
        }



        private void Update()
        {
            Ray cameraRay = _cam.ScreenPointToRay(Mouse.current.position.ReadValue());

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

        private void OnSmiteAbility(InputAction.CallbackContext context)
        {
            if (!_onCooldown)
            {
                GameObject sword = Instantiate(_sword,
                    new Vector3(transform.position.x, 1, transform.position.z),
                    Quaternion.LookRotation(_direction));

                StartCooldown();
            }
        }

        private void EnableWaveAbility()
        {
            _smiteAbility.Enable();
        }

        private async void StartCooldown()
        {
            _onCooldown = true;
            await UniTask.WaitForSeconds(_cooldown);
            _onCooldown = false;
        }
    }
}

