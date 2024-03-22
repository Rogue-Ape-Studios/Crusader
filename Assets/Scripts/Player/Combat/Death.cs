using RogueApeStudio.Crusader.HealthSystem;
using RogueApeStudio.Crusader.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using RogueApeStudio.Crusader.Player.Movement;

namespace RogueApeStudio.Crusader.Player.Combat
{
    public class Death : MonoBehaviour
    {
        [SerializeField] private Health _health;
        [SerializeField] private BasicAttack _basicAttack;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private Animator _animator;

        void Start()
        {
            _health.OnDeath += HandleOnDeath;
        }

        private void OnDestroy()
        {
            _health.OnDeath -= HandleOnDeath;
        }

        private void HandleOnDeath()
        {
            _playerController.ToggleInputActions();
            _basicAttack.SetCanAttack(false);
            _animator.SetTrigger("Death");
        }
    }
}
