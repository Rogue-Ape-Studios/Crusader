using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RogueApeStudio.Crusader.Player.Movement
{
    public class PlayerController : MonoBehaviour
    {
        private CrusaderInputActions _crusaderInputActions;
        private InputAction _movement;
        private int _moveSpeed = 5;
        private Vector3 _moveDirection = Vector3.zero;
        
        [SerializeField] private Rigidbody _rb;

        private void Awake()
        {
            _crusaderInputActions = new();
        }

        private void OnEnable()
        {
            _movement = _crusaderInputActions.Player.Move;
            EnableMovement();
        }

        private void OnDisable()
        {
            DisableMovement();
        }

        private void EnableMovement()
        {
            _movement.Enable();
        }

        private void DisableMovement()
        {
            _movement.Disable();
        }

        private void Move()
        {
            Vector2 inputDirection = _movement.ReadValue<Vector2>();

            if (inputDirection != Vector2.zero)
            {
                Vector3 movementDirection = new Vector3(inputDirection.x, 0f, inputDirection.y).normalized;

                // Calculate movement vector with the specified speed
                Vector3 movement = _moveSpeed * Time.fixedDeltaTime * movementDirection;

                // Move the character using Rigidbody
                _rb.MovePosition(_rb.position + movement);
            }
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            Move();
        }

    }
}
