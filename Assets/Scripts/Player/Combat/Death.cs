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

        void Start()
        {
            _health.OnDeath += HandleDeath;
        }

        private void OnDestroy()
        {
            _health.OnDeath -= HandleDeath;
        }

        private void HandleDeath()
        {
            _playerController.ToggleInputActions();
            _basicAttack.SetCanAttack(false);
        }
    }
}
